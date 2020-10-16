using MS.AFORO255.Cross.RabbitMQ.Src.Events;
using System.Threading.Tasks;

namespace MS.AFORO255.Cross.RabbitMQ.Src.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
         where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
