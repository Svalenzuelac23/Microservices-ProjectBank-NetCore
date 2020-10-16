using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.History.Model;
using MS.AFORO255.History.RabbitMQ.Events;
using MS.AFORO255.History.Service;
using System.Threading.Tasks;

namespace MS.AFORO255.History.RabbitMQ.EventHandlers
{
    public class WithdrawalEventHandler : IEventHandler<WithdrawalCreatedEvent> //ESTA INTERFAZ ESTA EN EL PROYECTO DE RABBIT PERMITE TRABAJAR CON LOS EVENTOS DE ESA COLA
    {
        private readonly IHistoryService _historyService;

        public WithdrawalEventHandler(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        //CADA VEZ QUE ENCONTREMOS UN EVENTO EN LA COLA DE TIPO DEPOSITCREATEDEVENT LO CAPTURARA ESTE MANEJADOR Y VA HACER UN REGISTRO EN LA BASE DE DATOS
        public Task Handle(WithdrawalCreatedEvent @event)
        {
            _historyService.Add(new HistoryTransaction()
            {
                IdTransaction = @event.IdTransaction,
                Amount = @event.Amount,
                Type = @event.Type,
                CreationDate = @event.CreationDate,              
                AccountId = @event.AccountId
            });

            return Task.CompletedTask;
        }
    }
}
