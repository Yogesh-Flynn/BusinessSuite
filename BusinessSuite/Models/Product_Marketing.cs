using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Product_Marketing
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Products { get; set; }

        public int MarketingId { get; set; }
        public Marketing Marketing { get; set; }
    }
}
