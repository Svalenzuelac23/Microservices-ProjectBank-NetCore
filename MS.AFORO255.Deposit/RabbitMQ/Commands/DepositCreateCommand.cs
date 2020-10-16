using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.Deposit.RabbitMQ.Commands
{
    public class DepositCreateCommand : DepositCommand
    {
        public DepositCreateCommand(int idTransaction, decimal amount, string type, string creationDate, int accountId)
        {
            IdTransaction = idTransaction;
            Amount = amount;
            Type = type;
            CreationDate = creationDate;
            AccountId = accountId;
        }
    }
}
