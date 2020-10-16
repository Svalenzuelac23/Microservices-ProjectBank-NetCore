using MS.AFORO255.Notification.Model;

namespace MS.AFORO255.Notification.Repository
{
    public interface IMailRepository
    {
        bool Add(SendMail sendMail);
    }
}
