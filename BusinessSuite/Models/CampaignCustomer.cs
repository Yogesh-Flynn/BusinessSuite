using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class CampaignCustomer
    {
        [Key]
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaigns Campaign { get; set; }

        public int CustomerId { get; set; }
        public Customers Customer { get; set; }
    }
}
