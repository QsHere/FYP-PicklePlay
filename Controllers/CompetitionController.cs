using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;
using Microsoft.EntityFrameworkCore; // Needed for Include
using System.Linq; // Needed for Select

namespace FYP_QS_CODE.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ApplicationDbContext _context; // Inject DbContext for Include

        public CompetitionController(IScheduleRepository scheduleRepository, ApplicationDbContext context)
        {
            _scheduleRepository = scheduleRepository;
            _context = context;
        }

        // GET: /Competition/Listing
        public IActionResult Listing()
        {
            // Fetch SCHEDULES of type Competition, including the related Competition data
            var competitionSchedules = _context.Schedules
                                      .Include(s => s.Competition) // Eager load Competition details
                                      .Where(s => s.ScheduleType == ScheduleType.Competition && s.Competition != null)
                                      .ToList(); // Fetch the data

            // --- Corrected Mapping Example ---
            // If you need a ViewModel, map it like this:
            var viewModelList = competitionSchedules.Select(schedule => new CompetitionListViewModel // Assuming you create this ViewModel
            {
                 // Access properties from the 'schedule' object
                 ScheduleId = schedule.ScheduleId, // Correct: Use ScheduleId from Schedule
                 Name = schedule.GameName,       // Correct: Use GameName from Schedule
                 Description = schedule.Description, // Correct: Use Description from Schedule
                 Location = schedule.Location,     // Correct: Use Location from Schedule
                 StartTime = schedule.StartTime,   // Correct: Use StartTime from Schedule
                 EndTime = schedule.EndTime,       // Correct: Use EndTime from Schedule
                 Status = schedule.Status,         // Correct: Use Status from Schedule
                 FeeAmount = schedule.FeeAmount,   // Correct: Use FeeAmount from Schedule

                 // Access competition-specific properties via navigation property
                 Format = schedule.Competition.Format, // Correct: Use Format from schedule.Competition

                 // CommunityId = schedule.CommunityId, // Add CommunityId to Schedule model if needed
                 // CreatedBy = schedule.CreatedByUserId // Add CreatedByUserId to Schedule model if needed

            }).ToList();

            // Pass the list of ViewModels (or the original Schedule objects if your view handles it)
            return View(viewModelList);
            // Or: return View(competitionSchedules);
        }

        // GET: /Competition/CompDetails/{id}
        public IActionResult CompDetails(int id)
        {
            // Fetch the specific SCHEDULE, ensuring it's a Competition and include Competition data
            var schedule = _context.Schedules
                                  .Include(s => s.Competition) // Eager load Competition details
                                  .FirstOrDefault(s => s.ScheduleId == id
                                                   && s.ScheduleType == ScheduleType.Competition
                                                   && s.Competition != null); // Ensure Competition data exists

            if (schedule == null)
            {
                return NotFound();
            }

            // --- Pass the Schedule object to the view ---
            // The view can access properties like:
            // @Model.GameName (from Schedule)
            // @Model.Location (from Schedule)
            // @Model.StartTime (from Schedule)
            // @Model.Competition.Format (from Competition)
            // @Model.Competition.NumPool (from Competition)
            return View(schedule);
        }

         // Placeholder ViewModel for Listing example
        public class CompetitionListViewModel
        {
            public int ScheduleId { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Location { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public ScheduleStatus? Status { get; set; }
            public decimal? FeeAmount { get; set; }
            public CompetitionFormat Format { get; set; }
            // public int? CommunityId { get; set; }
            // public string? CreatedBy { get; set; }
        }
    }
}