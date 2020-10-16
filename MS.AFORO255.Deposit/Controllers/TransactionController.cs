using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.Deposit.DTO;
using MS.AFORO255.Deposit.RabbitMQ.Commands;
using MS.AFORO255.Deposit.Service;
using System;

namespace MS.AFORO255.Deposit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IEventBus _bus;
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public TransactionController(IEventBus bus, ITransactionService transactionService, IAccountService accountService)
        {
            _bus = bus;
            _transactionService = transactionService;
            _accountService = accountService;
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest request)
        {
            //ESTA COMUNICACION ESPERA RESPUESTA ES MEDIANTE EL PROTOCOLO HTTP
            Model.Transaction transaction = new Model.Transaction()
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CreationDate = DateTime.Now.ToShortDateString(),
                Type = "Deposit"
            };

            transaction = _transactionService.Deposit(transaction);

            //ACA ESTAMOS ACTUALIZANDO LA INFORMACION DEL MICROSERVICIO ACCOUNT

            bool isProcess = _accountService.Execute(transaction);
            if (isProcess)
            {
                //ESTE OTRO TIPO DE COMUNICACION NO ESPERA RESPUESTA Y SE ESTA TRABAJANDO CON EL PROTOCOLO AMQP
                //ESTE SOLO LANZA EL COMANDO Y LISTO
                var createCommand = new DepositCreateCommand(
                    idTransaction: transaction.Id,
                    amount: transaction.Amount,
                    creationDate: transaction.CreationDate,
                    type: transaction.Type,
                    accountId: transaction.AccountId
                    );

                _bus.SendCommand(createCommand);

                //ACA ESTAMOS MANDANDO EL MENSAJE A LA COLA DE NOTIFICACIONES
                var createCommandNotification = new NotificationCreateCommand(
                    idTransaction: transaction.Id,
                    amount: transaction.Amount,
                    type: transaction.Type,
                    accountId: transaction.AccountId
                    );

                _bus.SendCommand(createCommandNotification);
            }

            /*END ACTUALIZACION*/            

            return Ok();
        }
    }
}