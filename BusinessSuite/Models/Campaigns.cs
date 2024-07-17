using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Campaigns
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Campaign_Customer> CampaignCustomers { get; set; }

        public ICollection<Campaign_Marketing> MarketingCampaigns { get; set; }
    }
}
