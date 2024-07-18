using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Campaigns_Customers
    {
        public int CampaignsId { get; set; }
        public Campaigns Campaign { get; set; }

        public int CustomersId { get; set; }
        public Customers Customer { get; set; }
    }
}
