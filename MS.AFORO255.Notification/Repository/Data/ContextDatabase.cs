using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Notification.Model;

namespace MS.AFORO255.Notification.Repository.Data
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }

        public DbSet<SendMail> SendMail { get; set; }
        public DbContext Instance => this;
    }
}
