using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Security.Model;

namespace MS.AFORO255.Security.Repository.Data
{
    public interface IContextDatabase
    {
        DbSet<Access> Access { get; set; }
    }
}
