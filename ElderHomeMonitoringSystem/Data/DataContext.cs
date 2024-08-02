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
        public DbSet<MovementLog> MovementLogs { get; set; }
        public DbSet<Elder> Elders { get; set; }
        public DbSet<ElderCares> ElderCares { get; set; }
        public DbSet<SittingPosture> SittingPostures { get; set; }
        public DbSet<SleepSession> SleepSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Elder>()
            .HasIndex(u => u.MacAddress)
            .IsUnique();

            modelBuilder.Entity<User>()
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
                .HasOne(e => e.Elder)
                .WithMany()
                .HasForeignKey(e => e.ElderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ElderCares>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
