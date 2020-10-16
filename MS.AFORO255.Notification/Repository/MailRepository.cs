using MS.AFORO255.Notification.Model;
using MS.AFORO255.Notification.Repository.Data;

namespace MS.AFORO255.Notification.Repository
{
    public class MailRepository : IMailRepository
    {
        private readonly IContextDatabase _contextDatabase;

        public MailRepository(IContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public bool Add(SendMail sendMail)
        {
            _contextDatabase.SendMail.Add(sendMail);
            _contextDatabase.SaveChanges();
            return true;
        }
    }
}
