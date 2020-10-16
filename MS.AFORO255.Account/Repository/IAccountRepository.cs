using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.Account.Repository
{
    public interface IAccountRepository
    {
        IEnumerable<Model.Account> GetAll();
        bool Deposit(Model.Account account);
        bool Withdrawal(Model.Account account);
    }
}
