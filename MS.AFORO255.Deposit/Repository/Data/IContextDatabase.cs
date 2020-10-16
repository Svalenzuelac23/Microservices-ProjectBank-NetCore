using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Deposit.Model;

namespace MS.AFORO255.Deposit.Repository.Data
{
    public interface IContextDatabase
    {
        DbSet<Transaction> Transaction { get; set; }
        int SaveChanges();
    }
}
