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
        public DbSet<Campaign_Customer> Campaign_Customers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Marketing_Product> Marketings_Product { get; set; }
        public DbSet<Campaign_Marketing> Campaigns_Marketing { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Campaign_Customer>()
                .HasKey(cc => new { cc.CampaignId, cc.CustomerId });

            modelBuilder.Entity<Campaign_Customer>()
                .HasOne(cc => cc.Campaign)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CampaignId);

            modelBuilder.Entity<Campaign_Customer>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CustomerId);
            
            
            
            
            modelBuilder.Entity<Marketing_Product>()
                .HasKey(cc => new { cc.ProductId, cc.MarketingId });

            modelBuilder.Entity<Marketing_Product>()
                .HasOne(cc => cc.Products)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.ProductId);

            modelBuilder.Entity<Marketing_Product>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.MarketingId);
            
            
            
            
            modelBuilder.Entity<Campaign_Marketing>()
                .HasKey(cc => new { cc.CampaignId, cc.MarketingId });

            modelBuilder.Entity<Campaign_Marketing>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.MarketingId);

            modelBuilder.Entity<Campaign_Marketing>()
                .HasOne(cc => cc.Campaigns)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.CampaignId);
        }

    }
}
