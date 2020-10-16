using MS.AFORO255.Deposit.Model;

namespace MS.AFORO255.Deposit.Repository
{
    public interface ITransactionRepository
    {
        Transaction Deposit(Transaction transaction);
        Transaction DepositReverse(Transaction transaction);
    }
}
