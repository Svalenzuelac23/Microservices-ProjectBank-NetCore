using MS.AFORO255.Deposit.Model;
using MS.AFORO255.Deposit.Repository.Data;

namespace MS.AFORO255.Deposit.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IContextDatabase _contextDatabase;

        public TransactionRepository(IContextDatabase contextDatabase )
        {
            _contextDatabase = contextDatabase;
        }

        public Transaction Deposit(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }

        public Transaction DepositReverse(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }
    }
}
