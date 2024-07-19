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
        public DbSet<Campaigns_Customers> Campaigns_Customers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Marketing_Products> Marketings_Products { get; set; }
        public DbSet<Campaigns_Marketings> Campaigns_Marketings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Messages> Messages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Campaigns_Customers>()
                .HasKey(cc => new { cc.CampaignsId, cc.CustomersId });

            modelBuilder.Entity<Campaigns_Customers>()
                .HasOne(cc => cc.Campaign)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CampaignsId);

            modelBuilder.Entity<Campaigns_Customers>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.CampaignCustomers)
                .HasForeignKey(cc => cc.CustomersId);
            
            
            
            
            modelBuilder.Entity<Marketing_Products>()
                .HasKey(cc => new { cc.ProductsId, cc.MarketingsId });

            modelBuilder.Entity<Marketing_Products>()
                .HasOne(cc => cc.Products)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.ProductsId);

            modelBuilder.Entity<Marketing_Products>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Product_Marketings)
                .HasForeignKey(cc => cc.MarketingsId);
            
            
            
            
            modelBuilder.Entity<Campaigns_Marketings>()
                .HasKey(cc => new { cc.CampaignsId, cc.MarketingsId });

            modelBuilder.Entity<Campaigns_Marketings>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.MarketingsId);

            modelBuilder.Entity<Campaigns_Marketings>()
                .HasOne(cc => cc.Campaigns)
                .WithMany(c => c.MarketingCampaigns)
                .HasForeignKey(cc => cc.CampaignsId);
        }

    }
}
