using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Marketing_Customers
    {
        [Key]
        public int Id { get; set; }
        public int MarketingsId { get; set; }
        public Marketing Marketing { get; set; }

        public int CustomersId { get; set; }
        public Customers Customer { get; set; }
    }
}
