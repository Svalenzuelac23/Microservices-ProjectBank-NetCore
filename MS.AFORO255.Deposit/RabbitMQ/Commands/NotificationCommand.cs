using MS.AFORO255.Cross.RabbitMQ.Src.Commands;

namespace MS.AFORO255.Deposit.RabbitMQ.Commands
{
    public class NotificationCommand : Command
    {
        public int IdTransaction { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Type { get; protected set; }
        public int AccountId { get; protected set; }
    }
}
