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
        public DbSet<Log> Logs { get; set; }
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
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
