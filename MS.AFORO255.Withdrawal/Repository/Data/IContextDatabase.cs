using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Withdrawal.Model;

namespace MS.AFORO255.Withdrawal.Repository.Data
{
    public interface IContextDatabase
    {
        DbSet<Transaction> Transaction { get; set; }
        int SaveChanges();
    }
}
