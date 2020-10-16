using MS.AFORO255.Deposit.Model;

namespace MS.AFORO255.Deposit.Service
{
    public interface ITransactionService
    {
        Transaction Deposit(Transaction transaction);
        Transaction DepositReverse(Transaction transaction);
    }
}
