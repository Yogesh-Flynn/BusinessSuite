using BusinessSuite.Models.Master_Models.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSuite.Models
{
    public class Marketing
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Products")]
        public int? ProductsId { get; set; }
        public Products Products { get; set; }

        public ICollection<Marketing_Customers> Marketings_Customers { get; set; }
        public ICollection<Campaigns_Marketings> Campaigns_Marketing { get; set; }

        // Property to store image as binary data
        public string? Image { get; set; }

        // Enum property to restrict to specific values
        public string TransitCarrier { get; set; }
    }

}
