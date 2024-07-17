using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public string UniqueCode { get; set; }
        public int Quantity { get; set; }

        public ICollection<Marketing_Product> Product_Marketings { get; set; }
    }
}
