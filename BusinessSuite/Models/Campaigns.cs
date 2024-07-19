using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Campaigns
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ICollection<Campaigns_Customers> CampaignCustomers { get; set; }

        public ICollection<Campaigns_Marketings> MarketingCampaigns { get; set; }
    }
}
