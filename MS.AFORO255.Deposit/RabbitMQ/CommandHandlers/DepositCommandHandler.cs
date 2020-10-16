using MediatR;
using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.Deposit.RabbitMQ.Commands;
using MS.AFORO255.Deposit.RabbitMQ.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MS.AFORO255.Deposit.RabbitMQ.CommandHandlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCreateCommand, bool>
    {
        private readonly IEventBus _bus; //ESTA ES LA COLA, EL BUS ES LA COLA DE RABBITMQ

        public DepositCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }

        //CADA VEZ QUE YO LANCE UN COMANDO ESTE LO VA A CAPTURAR, HAY QUE HACER UNA CONFIG EN EL STARTUP
        public Task<bool> Handle(DepositCreateCommand request, CancellationToken cancellationToken)
        {
            _bus.Publish(new DepositCreatedEvent(
                request.IdTransaction,
                request.Amount,
                request.Type,
                request.CreationDate,
                request.AccountId
                ));

            return Task.FromResult(true); //COMO EL METODO ES UNA TAREA, DEBO RESPONDER EL MISMO TIPO DE LA TAREA
        }
    }
}
