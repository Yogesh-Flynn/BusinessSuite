using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSuite.Models
{
    public class Marketing_Customers
    {
        // Foreign key for Marketing
        public int MarketingsId { get; set; }

        [ForeignKey("MarketingsId")]
        public Marketing Marketing { get; set; }

        // Foreign key for Customer
        public int CustomersId { get; set; }

        [ForeignKey("CustomersId")]
        public Customers Customer { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
