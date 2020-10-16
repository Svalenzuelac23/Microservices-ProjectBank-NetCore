using System.ComponentModel.DataAnnotations;

namespace MS.AFORO255.Account.Model
{
    public class Customer
    {
        [Key]
        public int IdCustomer { get; set; }
        public string FullName { get; set; }
    }
}
