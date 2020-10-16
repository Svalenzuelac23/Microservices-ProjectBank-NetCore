using MS.AFORO255.Cross.RabbitMQ.Src.Events;

namespace MS.AFORO255.History.RabbitMQ.Events
{
    public class DepositCreatedEvent : Event
    {
        //CON ESTA NOMBRE ENCOTNRAREMOS EL EVENTO EN LA COLA DE RABBIT
        public DepositCreatedEvent(int idTransaction, decimal amount, string type, string creationDate, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            CreationDate = creationDate;
            AccountId = accountId;
        }

        public int IdTransaction { get; set; }        
        public decimal Amount { get; set; }        
        public string Type { get; set; }       
        public string CreationDate { get; set; }        
        public int AccountId { get; set; }
    }
}
