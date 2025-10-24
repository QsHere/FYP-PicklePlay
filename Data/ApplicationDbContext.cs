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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions to strings for MySQL
            modelBuilder.Entity<Schedule>()
                .Property(s => s.ScheduleType)
                .HasConversion<string>();
                
            modelBuilder.Entity<Schedule>()
                .Property(s => s.EventTag)
                .HasConversion<string>();

            

            modelBuilder.Entity<Schedule>()
                .Property(s => s.GenderRestriction)
                .HasConversion<string>();
            
            modelBuilder.Entity<Schedule>()
                .Property(s => s.AgeGroupRestriction)
                .HasConversion<string>();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.FeeType)
                .HasConversion<string>();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Privacy)
                .HasConversion<string>();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.GameFeature)
                .HasConversion<string>();

        

            modelBuilder.Entity<Schedule>()
                .Property(s => s.HostRole)
                .HasConversion<string>();
                
                modelBuilder.Entity<Schedule>()
                .Property(s => s.Status)
                .HasConversion<string>();
        }
    }
}