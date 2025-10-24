using System;
using System.Collections.Generic;
using System.Linq;
using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Data
{
    public class MySqlScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public MySqlScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Schedule s)
        {
            _context.Schedules.Add(s);
            _context.SaveChanges();
        }

        public IEnumerable<Schedule> All()
        {
            // We set a default status filter to only show active games
            return _context.Schedules
                .Where(s => s.Status == ScheduleStatus.Active || s.Status == ScheduleStatus.Null)
                .OrderByDescending(s => s.StartTime)
                .ToList();
        }

        // Updated to use int and find by ScheduleId
        public Schedule? Get(int id)
        {
            return _context.Schedules.FirstOrDefault(s => s.ScheduleId == id);
        }

        public void Update(Schedule s)
        {
            _context.Schedules.Update(s);
            _context.SaveChanges();
        }

        // All JoinRequest and Seed methods are removed, matching the interface.
    }
}