using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<IdTracker> IdTrackers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).HasColumnType("nvarchar(max)").IsRequired(false);
                entity.Property(e => e.Level).HasColumnType("nvarchar(max)").IsRequired(false);
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
                entity.Property(e => e.Exception).HasColumnType("nvarchar(max)").IsRequired(false);
                entity.Property(e => e.Properties).HasColumnType("nvarchar(max)").IsRequired(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(450)").IsRequired();
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.MRP).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.WholesalePrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Stock).HasColumnType("int").IsRequired();
                entity.Property(e => e.LowStock).HasColumnType("int").IsRequired();
                entity.Property(e => e.DamageStock).HasColumnType("int").IsRequired();
                entity.Property(e => e.ImageUrl).HasColumnType("nvarchar(450)").IsRequired(false);
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.UnitId).IsRequired();

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Unit)
                    .WithMany(e=>e.Products)
                    .HasForeignKey(e => e.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.CompanyName).HasColumnType("nvarchar(100)").IsRequired(false);
                entity.Property(e => e.Mobile).HasColumnType("nvarchar(15)").IsRequired();
                entity.Property(e => e.Address).HasColumnType("nvarchar(200)").IsRequired(false);
                entity.Property(e => e.Email).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.OpeningBalance).HasColumnType("int").IsRequired();
                entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
            });

            modelBuilder.Entity<IdTracker>(entity =>
            {
                entity.HasKey(e => e.Prefix);
                entity.Property(e => e.LastUsedNumber).IsRequired();
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.OrderDate).HasColumnType("date").IsRequired();
                entity.Property(e => e.OrderTime).HasColumnType("time").IsRequired();
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Due).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Paid).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.PaymentStatus).HasConversion<int>().IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.HasOne(e => e.Customer)
                    .WithMany(e=>e.Sales)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Product)
                    .WithMany(e=>e.Sales)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
