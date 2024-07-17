﻿using System.ComponentModel.DataAnnotations;

namespace BusinessSuite.Models
{
    public class Marketing
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Product_Marketing> Product_Marketings { get; set; }
        public ICollection<MarketingCampaign> MarketingCampaigns { get; set; }
    }
}
