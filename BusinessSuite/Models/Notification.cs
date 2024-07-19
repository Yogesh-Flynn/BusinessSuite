using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign key property
        public int CampaignId { get; set; }

        // Navigation property
        public Campaigns Campaign { get; set; }
    }
}
