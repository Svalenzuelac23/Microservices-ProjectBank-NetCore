using MS.AFORO255.Withdrawal.Model;
using MS.AFORO255.Withdrawal.Repository;

namespace MS.AFORO255.Withdrawal.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Transaction Withdrawal(Transaction transaction)
        {
            return _transactionRepository.Withdrawal(transaction);
        }

        public Transaction WithdrawalReverse(Transaction transaction)
        {
            return _transactionRepository.WithdrawalReverse(transaction);
        }
    }
}
