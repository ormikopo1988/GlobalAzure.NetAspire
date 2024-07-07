using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using GlobalAzure.NetAspire.Server.Data.Entities;

namespace GlobalAzure.NetAspire.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("User", "identity");

            modelBuilder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("UserLogin", "identity");

            modelBuilder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("UserClaim", "identity");

            modelBuilder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("RoleClaim", "identity");

            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("UserRole", "identity");

            modelBuilder.Entity<IdentityRole<Guid>>()
                .ToTable("Role", "identity");

            modelBuilder.Entity<IdentityUserToken<Guid>>()
                .ToTable("UserToken", "identity");

            modelBuilder.Entity<Customer>()
                .HasKey(k => k.Id);
        }
    }
}
