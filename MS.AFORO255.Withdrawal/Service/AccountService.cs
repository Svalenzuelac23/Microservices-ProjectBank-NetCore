using Microsoft.Extensions.Configuration;
using MS.AFORO255.Cross.Proxy.Proxy;
using MS.AFORO255.Withdrawal.DTO;
using MS.AFORO255.Withdrawal.Model;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading.Tasks;

namespace MS.AFORO255.Withdrawal.Service
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

        public async Task<bool> WithdrawalAccount(AccountRequest request)
        {
            string uri = _configuration["proxy:urlAccountWithdrawal"];
            var response =await _httpClient.PostAsync(uri, request);
            response.EnsureSuccessStatusCode();
            return true;
        }

        public bool WithdrawalReverse(Transaction request)
        {
            _transactionService.WithdrawalReverse(request);
            return true;
        }

        public bool Execute(Transaction request)
        {
            bool response = false;

            //ESTA ES LA POLITICA PARA EL PATRON CIRCUIT BREAKER
            var circuitBreakerPolicy = Policy.Handle<Exception>().
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
                .Wrap(circuitBreakerPolicy); //ACA ESTOS SOBREPONIENTO LA POLITICA DE CIRCUIT BREAKER SOBRE LA DE REITENTOS

            retry.Execute(() =>
            {
                if (circuitBreakerPolicy.CircuitState == CircuitState.Closed)
                {
                    circuitBreakerPolicy.Execute(() =>
                    {
                        AccountRequest account = new AccountRequest()
                        {
                            Amount = request.Amount,
                            IdAccount = request.AccountId
                        };
                        response = WithdrawalAccount(account).Result;
                        Console.WriteLine("Solicitud realizada con exito");
                    });
                }

                if(circuitBreakerPolicy.CircuitState != CircuitState.Closed) //Despues de los intentos que le dijimos se va abrir el circuito
                {
                    Transaction transaction = new Transaction()
                    {
                        AccountId = request.AccountId,
                        Amount = request.Amount,
                        CreationDate = DateTime.Now.ToShortDateString(),                        
                        Type = "With R"
                    };

                    response = WithdrawalReverse(transaction);
                    response = false;
                }
            });

            return response;
        }
    }
}
