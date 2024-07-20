using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Marketing
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public Products Products { get; set; }

        public ICollection<Marketing_Customers> Marketings_Customers { get; set; }

        public ICollection<Campaigns_Marketings> Campaigns_Marketing { get; set; }

    }
}
