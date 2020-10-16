namespace MS.AFORO255.Deposit.RabbitMQ.Commands
{
    public class NotificationCreateCommand : NotificationCommand
    {
        public NotificationCreateCommand(int idTransaction, decimal amount, string type, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            AccountId = accountId;
        }
    }
}
