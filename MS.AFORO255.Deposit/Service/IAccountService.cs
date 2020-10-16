using MS.AFORO255.Deposit.DTO;
using MS.AFORO255.Deposit.Model;
using System.Threading.Tasks;

namespace MS.AFORO255.Deposit.Service
{
    public interface IAccountService
    {
        Task<bool> DepositAccount(AccountRequest request);
        bool DepositReverse(Transaction request);
        bool Execute(Transaction request); //Este metodo es el que va a evaluar que hacer con el deposito, si es exito usara Deposit Account si falla hara el RollBack con deposit reverse
    }
}
