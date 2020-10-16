using MS.AFORO255.Withdrawal.Model;

namespace MS.AFORO255.Withdrawal.Repository
{
    public interface ITransactionRepository
    {
        Transaction Withdrawal(Transaction transaction);
        Transaction WithdrawalReverse(Transaction transaction);
    }
}
