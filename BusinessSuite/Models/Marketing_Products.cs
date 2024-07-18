using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Marketing_Products
    {
        public int ProductsId { get; set; }
        public Products Products { get; set; }

        public int MarketingsId { get; set; }
        public Marketing Marketing { get; set; }
    }
}
