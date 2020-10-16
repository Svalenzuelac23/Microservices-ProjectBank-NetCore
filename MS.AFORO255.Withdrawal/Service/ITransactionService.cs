using MS.AFORO255.Withdrawal.Model;

namespace MS.AFORO255.Withdrawal.Service
{
    public interface ITransactionService
    {
        Transaction Withdrawal(Transaction transaction);
        Transaction WithdrawalReverse(Transaction transaction);
    }
}
