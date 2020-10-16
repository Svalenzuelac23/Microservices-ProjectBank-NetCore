using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Account.Model;

namespace MS.AFORO255.Account.Repository.Data
{
    public interface IContextDatabase
    {
        DbSet<Model.Account> Account { get; set; }
        DbSet<Customer> Customer { get; set; }
        int SaveChanges();
    }
}
