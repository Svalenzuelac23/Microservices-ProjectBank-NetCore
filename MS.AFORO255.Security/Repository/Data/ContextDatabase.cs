using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Security.Model;

namespace MS.AFORO255.Security.Repository.Data
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options) //ACA LE ESTMOS PASANDO DESDE EL CONTRUCTUR EL VALOR DE OPTIONES AL PARAMETRO DE LA CLASE BASE DBCONTEXT
        {
        }

        public DbSet<Access> Access { get; set; }
    }
}
