using BusinessSuite.Models;
using BusinessSuite.Models.Master_Models;
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
        public DbSet<ColumnMaster> ColumnMasters { get; set; }
        public DbSet<DatabaseMaster> DatabaseMasters { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PermissionMaster> PermissionMasters { get; set; }
        public DbSet<TableMaster> TableMasters { get; set; }
        public DbSet<Website> Websites { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }

    } 
    public class CRMDbContext : DbContext
    {
        public CRMDbContext(DbContextOptions<CRMDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Campaigns> Campaigns { get; set; }
        public DbSet<Marketing_Customers> Marketings_Customers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Campaigns_Marketings> Campaigns_Marketings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Marketing_Customers>()
                .HasKey(cc => new { cc.MarketingsId, cc.CustomersId });

            modelBuilder.Entity<Marketing_Customers>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Marketings_Customers)
                .HasForeignKey(cc => cc.MarketingsId);

            modelBuilder.Entity<Marketing_Customers>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.Marketing_Customers)
                .HasForeignKey(cc => cc.CustomersId);
            
            
            modelBuilder.Entity<Campaigns_Marketings>()
                .HasKey(cc => new { cc.CampaignsId, cc.MarketingsId });

            modelBuilder.Entity<Campaigns_Marketings>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Campaigns_Marketing)
                .HasForeignKey(cc => cc.MarketingsId);

            modelBuilder.Entity<Campaigns_Marketings>()
                .HasOne(cc => cc.Campaigns)
                .WithMany(c => c.Campaigns_Marketings)
                .HasForeignKey(cc => cc.CampaignsId);

            modelBuilder.Entity<Products>()
            .HasMany(p => p.Marketings)
            .WithOne(m => m.Products)
            .HasForeignKey(m => m.ProductsId)
            .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
