using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Campaigns
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CampaignCustomer> CampaignCustomers { get; set; }

        public ICollection<MarketingCampaign> MarketingCampaigns { get; set; }
    }
}
