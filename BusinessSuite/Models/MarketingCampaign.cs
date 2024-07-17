using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class MarketingCampaign
    {
        public int CampaignId { get; set; }
        public Campaigns Campaigns { get; set; }

        public int MarketingId { get; set; }
        public Marketing Marketing { get; set; }
    }
}
