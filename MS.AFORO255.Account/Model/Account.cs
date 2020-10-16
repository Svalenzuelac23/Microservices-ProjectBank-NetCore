using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.AFORO255.Account.Model
{
    public class Account
    {
        [Key]
        public int IdAccount { get; set; }
        public decimal TotalAmount { get; set; }
        [ForeignKey("Customer")] //ACA LE ESTAMOS DICIENDO QUE ESTA ES LA LLAVE FORANEA QUE SE RELACIONA CON EL ID DE LA TABLA CUSTOMER
        public int IdCustomer { get; set; }
        public Customer Customer { get; set; }
    }
}
