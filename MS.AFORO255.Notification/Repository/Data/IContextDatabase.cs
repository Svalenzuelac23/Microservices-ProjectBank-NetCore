using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Notification.Model;

namespace MS.AFORO255.Notification.Repository.Data
{
    public interface IContextDatabase
    {
        DbSet<SendMail> SendMail { get; set; }
        int SaveChanges();
    }
}
