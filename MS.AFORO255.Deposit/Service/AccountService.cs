using Microsoft.Extensions.Configuration;
using MS.AFORO255.Cross.Proxy.Proxy;
using MS.AFORO255.Deposit.DTO;
using MS.AFORO255.Deposit.Model;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading.Tasks;

namespace MS.AFORO255.Deposit.Service
{
    public class AccountService : IAccountService
    {
        private readonly ITransactionService _transactionService;
        private readonly IHttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountService(ITransactionService transactionService, IHttpClient httpClient, IConfiguration configuration)
        {
            _transactionService = transactionService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> DepositAccount(AccountRequest request)
        {
            string uri = _configuration["proxy:urlAccountDeposit"];
            var response =await _httpClient.PostAsync(uri, request);
            response.EnsureSuccessStatusCode();
            return true;
        }

        public bool DepositReverse(Transaction request)
        {
            _transactionService.DepositReverse(request);
            return true;
        }

        public bool Execute(Transaction request)
        {
            bool response = false;

            //ESTA ES LA POLITICA PARA EL PATRON CIRCUIT BREAKER
            var circuitBraakerPolicy = Policy.Handle<Exception>().
                CircuitBreaker(3, TimeSpan.FromSeconds(10),
                onBreak: (ex, timespan, context) =>
                {
                    Console.WriteLine("El circuito entro en estado de falla");
                }, onReset: (context) =>
                {
                    Console.WriteLine("El circuito dejo estado de falla");
                });

            //POLITICA DE POLLY / REINTENTOS
            var retry = Policy.Handle<Exception>()
                .WaitAndRetryForever(attemp => TimeSpan.FromMilliseconds(300))
                .Wrap(circuitBraakerPolicy); //ACA ESTOS SOBREPONIENTO LA POLITICA DE CIRCUIT BREAKER SOBRE LA DE REITENTOS

            retry.Execute(() =>
            {
                if (circuitBraakerPolicy.CircuitState == CircuitState.Closed)
                {
                    circuitBraakerPolicy.Execute(() =>
                    {
                        AccountRequest account = new AccountRequest()
                        {
                            Amount = request.Amount,
                            IdAccount = request.AccountId
                        };
                        response = DepositAccount(account).Result;
                        Console.WriteLine("Solicitud realizada con exito");
                    });
                }

                if(circuitBraakerPolicy.CircuitState != CircuitState.Closed) //Despues de los intentos que le dijimos se va abrir el circuito
                {
                    Transaction transaction = new Transaction()
                    {
                        AccountId = request.AccountId,
                        Amount = request.Amount,
                        CreationDate = DateTime.Now.ToShortDateString(),                        
                        Type = "Deposit R"
                    };

                    response = DepositReverse(transaction);
                    response = false;
                }
            });

            return response;
        }
    }
}
