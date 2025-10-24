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

            // All .HasConversion<string>() calls have been removed.
            // EF Core will now correctly save the enums as integers (TINYINT)
            // by default.
        }
    }
}