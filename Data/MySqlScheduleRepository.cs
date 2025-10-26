using FYP_QS_CODE.Models;
using Microsoft.EntityFrameworkCore;

namespace FYP_QS_CODE.Data
{
    public class MySqlScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public MySqlScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Schedule> All()
        {
            return _context.Schedules.ToList();
        }

        public Schedule? GetById(int id)
        {
            return _context.Schedules.FirstOrDefault(s => s.ScheduleId == id);
        }

        public void Add(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }

        // --- ADD THESE METHODS ---
        public void Update(Schedule schedule)
        {
            // Mark the entity as modified
            _context.Entry(schedule).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var scheduleToDelete = _context.Schedules.Find(id);
            if (scheduleToDelete != null)
            {
                _context.Schedules.Remove(scheduleToDelete);
                _context.SaveChanges();
            }
            // Optional: Handle case where schedule is not found (throw exception, return bool, etc.)
        }
        // --- END ADD ---
    }
}