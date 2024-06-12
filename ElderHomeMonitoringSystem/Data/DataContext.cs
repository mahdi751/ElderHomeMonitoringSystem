using ElderHomeMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderHomeMonitoringSystem.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Elder>()
            .HasIndex(u => u.MacAddress)
            .IsUnique();

            modelBuilder.Entity<Elder>()
            .HasIndex(u => u.ElderCode)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

            modelBuilder.Entity<ElderCares>()
              .HasKey(ec => new { ec.UserId, ec.ElderId });
        }

    }
}
