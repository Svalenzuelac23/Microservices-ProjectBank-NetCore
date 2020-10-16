using MS.AFORO255.Cross.RabbitMQ.Src.Commands;
using MS.AFORO255.Cross.RabbitMQ.Src.Events;
using System.Threading.Tasks;

namespace MS.AFORO255.Cross.RabbitMQ.Src.Bus
{
    //ESTA ES LA COLA DE RABBITMQ
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
