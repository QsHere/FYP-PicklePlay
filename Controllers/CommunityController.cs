using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;
using System;
using System.Linq; // Keep this if needed elsewhere in the controller
using Microsoft.EntityFrameworkCore; // Keep this if needed elsewhere

namespace FYP_QS_CODE.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        // You only need ApplicationDbContext here if CommunityController uses it directly
        // private readonly ApplicationDbContext _context;

        // Correct constructor for CommunityController
        public CommunityController(IScheduleRepository scheduleRepository /*, ApplicationDbContext context - Add if needed */)
        {
            _scheduleRepository = scheduleRepository;
            // _context = context; // Assign if needed
        }


        // Community Home Page
        public IActionResult Index()
        {
             // If _scheduleRepository.All() works, keep it.
             // If you need filtering/including, use _context:
             // var games = _context.Schedules.Include(s => s.Competition).ToList();
            var games = _scheduleRepository.All();
            return View(games);
        }

        // --- CREATE RECURRING ---
        [HttpGet]
        public IActionResult CreateRecurring()
        {
            return View(new ScheduleRecurringViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRecurring(ScheduleRecurringViewModel vm)
        {
            var selectedDateTime = DateTime.Today.Add(vm.StartTime.ToTimeSpan());
            RecurringWeek todayDayFlag = RecurringWeek.None;
             switch (DateTime.Today.DayOfWeek) { /* Map DayOfWeek to RecurringWeek flags */
                 case DayOfWeek.Monday:    todayDayFlag = RecurringWeek.Mon; break;
                 case DayOfWeek.Tuesday:   todayDayFlag = RecurringWeek.Tue; break;
                 case DayOfWeek.Wednesday: todayDayFlag = RecurringWeek.Wed; break;
                 case DayOfWeek.Thursday:  todayDayFlag = RecurringWeek.Thur; break;
                 case DayOfWeek.Friday:    todayDayFlag = RecurringWeek.Fri; break;
                 case DayOfWeek.Saturday:  todayDayFlag = RecurringWeek.Sat; break;
                 case DayOfWeek.Sunday:    todayDayFlag = RecurringWeek.Sun; break;
             }

            if (vm.RecurringWeek != null && vm.RecurringWeek.Contains(todayDayFlag) && selectedDateTime <= DateTime.Now)
            {
                 ModelState.AddModelError("StartTime", "Please select a future time for today's recurring schedule.");
            }
             if (vm.RecurringWeek == null || !vm.RecurringWeek.Any()) {
                ModelState.AddModelError("RecurringWeek", "Please select at least one day.");
            }


            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            RecurringWeek combinedWeek = RecurringWeek.None;
            if (vm.RecurringWeek != null && vm.RecurringWeek.Count > 0)
            {
                foreach (var day in vm.RecurringWeek)
                {
                    combinedWeek |= day;
                }
            }

            var newSchedule = new Schedule
            {
                ScheduleType = Models.ScheduleType.Recurring,
                RecurringWeek = combinedWeek,
                AutoCreateWhen = vm.AutoCreateWhen,
                StartTime = DateTime.Today.Add(vm.StartTime.ToTimeSpan()),
                GameName = vm.GameName,
                Description = vm.Description,
                EventTag = vm.EventTag,
                Location = vm.Location,
                Duration = vm.Duration,
                NumPlayer = vm.NumPlayer,
                MinRankRestriction = vm.MinRankRestriction,
                MaxRankRestriction = vm.MaxRankRestriction,
                GenderRestriction = vm.GenderRestriction,
                AgeGroupRestriction = vm.AgeGroupRestriction,
                FeeType = vm.FeeType,
                FeeAmount = (vm.FeeType == FeeType.AutoSplitTotal || vm.FeeType == FeeType.PerPerson) ? vm.FeeAmount : null,
                Privacy = vm.Privacy,
                GameFeature = vm.GameFeature,
                CancellationFreeze = vm.CancellationFreeze,
                HostRole = vm.HostRole
            };

            if (newSchedule.StartTime.HasValue && newSchedule.Duration.HasValue)
            {
                var durationTimeSpan = ScheduleHelper.GetTimeSpan(newSchedule.Duration.Value);
                newSchedule.EndTime = newSchedule.StartTime.Value.Add(durationTimeSpan);
            }

            _scheduleRepository.Add(newSchedule);
            return RedirectToAction("Index", "Community");
        }

        // --- CREATE ONE-OFF ---
        [HttpGet]
        public IActionResult CreateOneOff()
        {
            return View(new ScheduleCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOneOff(ScheduleCreateViewModel vm)
        {
             if (vm.StartTime <= DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Please select a future date and time.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var newSchedule = new Schedule
            {
                ScheduleType = Models.ScheduleType.OneOff,
                GameName = vm.GameName,
                Description = vm.Description,
                EventTag = vm.EventTag,
                Location = vm.Location,
                StartTime = vm.StartTime,
                Duration = vm.Duration,
                NumPlayer = vm.NumPlayer,
                MinRankRestriction = vm.MinRankRestriction,
                MaxRankRestriction = vm.MaxRankRestriction,
                GenderRestriction = vm.GenderRestriction,
                AgeGroupRestriction = vm.AgeGroupRestriction,
                FeeType = vm.FeeType,
                FeeAmount = (vm.FeeType == FeeType.AutoSplitTotal || vm.FeeType == FeeType.PerPerson) ? vm.FeeAmount : null,
                Privacy = vm.Privacy,
                GameFeature = vm.GameFeature,
                CancellationFreeze = vm.CancellationFreeze,
                Repeat = vm.Repeat,
                HostRole = vm.HostRole
            };

            if (newSchedule.StartTime.HasValue && newSchedule.Duration.HasValue)
            {
                var durationTimeSpan = ScheduleHelper.GetTimeSpan(newSchedule.Duration.Value);
                newSchedule.EndTime = newSchedule.StartTime.Value.Add(durationTimeSpan);
            }

            _scheduleRepository.Add(newSchedule);
            return RedirectToAction("Index", "Community");
        }


        // --- CREATE COMPETITION ---
        // POST Action: Handles the form submission
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult CreateCompetition(ScheduleCompetitionViewModel vm)
{
    // --- Server-side validation (Keep existing date/fee checks) ---
    if (vm.EndTime <= vm.StartTime) { ModelState.AddModelError("EndTime", "End Date & Time must be after Start Date & Time."); }
    if (vm.RegClose <= vm.RegOpen) { ModelState.AddModelError("RegClose", "Registration Close Date must be after Registration Open Date."); }
    if (vm.RegClose >= vm.StartTime) { ModelState.AddModelError("RegClose", "Registration must close before the competition starts."); }
    if (vm.EarlyBirdClose.HasValue && vm.EarlyBirdClose.Value <= vm.RegOpen) { ModelState.AddModelError("EarlyBirdClose", "Early Bird Deadline must be after Registration Open Date."); }
    if (vm.EarlyBirdClose.HasValue && vm.EarlyBirdClose.Value >= vm.RegClose) { ModelState.AddModelError("EarlyBirdClose", "Early Bird Deadline must be before Registration Close Date."); }
    // Check FeeAmount only if FeeType selected corresponds to "Per Team" (which we map to PerPerson)
    if (vm.FeeType == FeeType.PerPerson && !vm.FeeAmount.HasValue) { ModelState.AddModelError("FeeAmount", "Fee Amount is required for Per Team fee type."); }
    if (vm.StartTime <= DateTime.Now) { ModelState.AddModelError("StartTime", "Competition Start Date & Time must be in the future."); }
    // Add check for RegOpen being in the future?
    // if (vm.RegOpen <= DateTime.Now) { ModelState.AddModelError("RegOpen", "Registration Open Date must be in the future."); }


    if (!ModelState.IsValid)
    {
        return View(vm);
    }

    // 1. Map ViewModel to Schedule Model
    var newSchedule = new Schedule
    {
        ScheduleType = ScheduleType.Competition,
        GameFeature = GameFeature.Ranking,
        GameName = vm.GameName,
        // EventTag = null, // Removed
        Description = vm.Description,
        Location = vm.Location,
        StartTime = vm.StartTime,
        EndTime = vm.EndTime,
        ApproxStartTime = vm.ApproxStartTime,
        RegOpen = vm.RegOpen,
        RegClose = vm.RegClose,
        EarlyBirdClose = vm.EarlyBirdClose,
        NumTeam = vm.NumTeam,
        Duration = null,
        NumPlayer = null,
        MinRankRestriction = vm.MinRankRestriction,
        MaxRankRestriction = vm.MaxRankRestriction,
        GenderRestriction = vm.GenderRestriction,
        AgeGroupRestriction = vm.AgeGroupRestriction,
        FeeType = vm.FeeType, // Still saving FeeType.Free or FeeType.PerPerson
        FeeAmount = (vm.FeeType == FeeType.PerPerson) ? vm.FeeAmount : null, // Amount only if PerPerson (Per Team)
        Privacy = vm.Privacy,
        CancellationFreeze = vm.CancellationFreeze,
        HostRole = HostRole.HostOnly, // <-- Set Default HostRole
        Status = ScheduleStatus.PendingSetup // <-- Set Status to PendingSetup
    };

    // 2. Create the linked Competition entity
    var newCompetition = new Competition
    {
        Format = vm.Format
        // Other fields remain default for now
    };

    // 3. Link them
    newSchedule.Competition = newCompetition;

    // 4. Add the Schedule to the database
    try
    {
        _scheduleRepository.Add(newSchedule);
        TempData["SuccessMessage"] = "Competition draft created! Proceed to setup matches.";
        // Still redirect to SetupMatch, passing the new ID
        return RedirectToAction("SetupMatch", new { id = newSchedule.ScheduleId });
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"An error occurred creating the competition: {ex.Message}");
        return View(vm);
    }
}
        // ------------------------


        // Setup Match Form
        public IActionResult SetupMatch(int id)
        {
            // You'll need to fetch the schedule (including competition data) here later
             var schedule = _scheduleRepository.GetById(id); // Ensure GetById includes Competition or use _context.Schedules.Include(...)
            if (schedule == null || schedule.ScheduleType != ScheduleType.Competition || schedule.Competition == null)
            {
                return NotFound(); // Or redirect with error
            }
             // Pass the schedule or a specific Setup ViewModel to the view
             // For now, just passing the ID
            ViewBag.ScheduleId = id;
            ViewBag.CompetitionFormat = schedule.Competition.Format; // Pass format to view

            return View(); // Consider passing the schedule object: return View(schedule);
        }
    }
}