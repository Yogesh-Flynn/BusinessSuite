using BusinessSuite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusinessSuite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Campaigns> Campaigns { get; set; }
        public DbSet<CampaignCustomer> CampaignCustomers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Product_Marketing> Product_Marketings { get; set; }
        public DbSet<MarketingCampaign> MarketingCampaigns { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CampaignCustomer>()
                .HasKey(cc => new { cc.CampaignId, cc.CustomerId });

            modelBuilder.Entity<CampaignCustomer>()
                .HasOne(cc => cc.Campaign)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CampaignId);

            modelBuilder.Entity<CampaignCustomer>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CustomerId);
            
            
            
            
            modelBuilder.Entity<Product_Marketing>()
                .HasKey(cc => new { cc.ProductId, cc.MarketingId });

            modelBuilder.Entity<Product_Marketing>()
                .HasOne(cc => cc.Products)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.ProductId);

            modelBuilder.Entity<Product_Marketing>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.MarketingId);
            
            
            
            
            modelBuilder.Entity<MarketingCampaign>()
                .HasKey(cc => new { cc.CampaignId, cc.MarketingId });

            modelBuilder.Entity<MarketingCampaign>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.MarketingId);

            modelBuilder.Entity<MarketingCampaign>()
                .HasOne(cc => cc.Campaigns)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.CampaignId);
        }

    }
}
