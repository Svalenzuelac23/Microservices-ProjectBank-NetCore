using MS.AFORO255.Cross.RabbitMQ.Src.Events;

namespace MS.AFORO255.Deposit.RabbitMQ.Events
{
    public class NotificationCreatedEvent : Event
    {
        public NotificationCreatedEvent(int idTransaction, decimal amount, string type, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            AccountId = accountId;
        }

        public int IdTransaction { get; set; }        
        public decimal Amount { get; set; }        
        public string Type { get; set; }         
        public int AccountId { get; set; }
    }
}
