using MS.AFORO255.Withdrawal.Model;
using MS.AFORO255.Withdrawal.Repository.Data;

namespace MS.AFORO255.Withdrawal.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IContextDatabase _contextDatabase;

        public TransactionRepository(IContextDatabase contextDatabase )
        {
            _contextDatabase = contextDatabase;
        }

        public Transaction Withdrawal(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }

        public Transaction WithdrawalReverse(Transaction transaction)
        {
            _contextDatabase.Transaction.Add(transaction);
            _contextDatabase.SaveChanges();
            return transaction;
        }
    }
}
