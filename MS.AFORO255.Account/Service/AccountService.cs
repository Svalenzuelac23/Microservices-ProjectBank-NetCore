using MS.AFORO255.Account.Repository;
using System.Collections.Generic;

namespace MS.AFORO255.Account.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool Deposit(Model.Account account)
        {
            return _accountRepository.Deposit(account);
        }

        public IEnumerable<Model.Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public bool Withdrawal(Model.Account account)
        {
            return _accountRepository.Withdrawal(account);
        }
    }
}
