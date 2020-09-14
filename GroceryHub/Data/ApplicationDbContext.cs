using System;
using System.Collections.Generic;
using System.Text;
using GroceryHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GroceryHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroceryHub.Models.Store> Store { get; set; }
        public DbSet<GroceryHub.Models.Offer> Offer { get; set; }
        public DbSet<GroceryHub.Models.Item> Item { get; set; }
        public DbSet<GroceryHub.Models.Category> Category { get; set; }
        public DbSet<GroceryHub.Models.CartItem> CartItem { get; set; }
        public DbSet<GroceryHub.Models.Cart> Cart { get; set; }
        public DbSet<GroceryHub.Models.Advertisment> Advertisment { get; set; }
        public DbSet<GroceryHub.Models.FlyerReader> FlyerReader { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>()
                .HasOne(a => a.Cart)
                .WithOne(b => b.AppUser)
                .HasForeignKey<Cart>(b => b.AppUserID);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FlyerReader>()
                .HasOne(a => a.Store);

            modelBuilder.Entity<FlyerReader>()
                .HasMany(a => a.flyerOffers)
                .WithOne(b => b.flyerReader);

            modelBuilder.Ignore<SelectListGroup>();
        }
    }
}
