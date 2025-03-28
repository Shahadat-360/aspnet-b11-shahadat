using DevSkill.Inventory.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Log> Logs { get; }

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
        }
    }
}
