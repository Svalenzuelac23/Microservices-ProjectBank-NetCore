using MS.AFORO255.Cross.RabbitMQ.Src.Events;
using System;

namespace MS.AFORO255.Cross.RabbitMQ.Src.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
