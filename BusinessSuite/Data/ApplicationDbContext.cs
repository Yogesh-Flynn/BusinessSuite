﻿using BusinessSuite.Models;
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
        public DbSet<Marketing_Customers> Marketings_Customers { get; set; }
        public DbSet<Marketing> Marketings { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Campaigns_Marketings> Campaigns_Marketings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Messages> Messages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Marketing_Customers>()
                .HasKey(cc => new { cc.MarketingId, cc.CustomersId });

            modelBuilder.Entity<Marketing_Customers>()
                .HasOne(cc => cc.Marketing)
                .WithMany(c => c.Marketings_Customers)
                .HasForeignKey(cc => cc.MarketingId);

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
           .HasOne(p => p.Marketing)
           .WithOne(m => m.Products)
           .HasForeignKey<Marketing>(m => m.ProductId);
        }

    }
}
