using FYP_QS_CODE.Models;
using Microsoft.EntityFrameworkCore;

namespace FYP_QS_CODE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Competition> Competitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- CONFIGURE ONE-TO-ONE RELATIONSHIP ---
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Competition) // Schedule has one Competition (or null)
                .WithOne(c => c.Schedule)   // Competition has one required Schedule
                .HasForeignKey<Competition>(c => c.ScheduleId); // The FK is in Competition table
            // --- END CONFIGURATION ---

            // --- Configure Enums for Competition (if needed) ---
            modelBuilder.Entity<Competition>()
                .Property(c => c.Format)
                .HasConversion<byte>(); // Convert TINYINT to byte enum

            modelBuilder.Entity<Competition>()
                .Property(c => c.StandingCalculation)
                .HasConversion<byte>();

            // --- Existing Enum Conversions for Schedule (keep these) ---
            // modelBuilder.Entity<Schedule>()...
        }
    }
}