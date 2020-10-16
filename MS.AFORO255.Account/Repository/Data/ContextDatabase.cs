using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Account.Model;

namespace MS.AFORO255.Account.Repository.Data
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) 
            : base(options) //ACA LE ESTMOS PASANDO DESDE EL CONTRUCTUR EL VALOR DE OPTIONES AL PARAMETRO DE LA CLASE BASE DBCONTEXT
        {
        }

        public DbSet<Model.Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbContext Instance => this; //ESAT ES UNA INSTANCIA DEL MISMO CONTEXTO
    }
}
