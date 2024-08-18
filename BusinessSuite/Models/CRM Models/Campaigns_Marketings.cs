using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Campaigns_Marketings
    {
        [Key]
        public int Id { get; set; }
        public int CampaignsId { get; set; }
        public Campaigns Campaigns { get; set; }

        public int MarketingsId { get; set; }
        public Marketing Marketing { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
