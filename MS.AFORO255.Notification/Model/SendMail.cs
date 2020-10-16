using System.ComponentModel.DataAnnotations;

namespace MS.AFORO255.Notification.Model
{
    public class SendMail
    {
        [Key]
        public int SendMailId { get; set; }
        public string SendDate { get; set; }
        public int AccountId { get; set; }
    }
}
