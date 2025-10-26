using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;
using System; 

namespace FYP_QS_CODE.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        public CommunityController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        // Community Home Page
        public IActionResult Index()
        {
            var games = _scheduleRepository.All();
            return View(games);
        }

        // --- CREATE RECURRING ---
        // GET Action: Shows the form
        [HttpGet]
        public IActionResult CreateRecurring()
        {
            // Pass a new, empty recurring view model
            return View(new ScheduleRecurringViewModel());
        }

        // POST Action: Handles the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRecurring(ScheduleRecurringViewModel vm)
        {
            // --- Server-side validation for future time ---
            var selectedDateTime = DateTime.Today.Add(vm.StartTime.ToTimeSpan());
            if (vm.RecurringWeek.Contains((RecurringWeek)(1 << (int)DateTime.Today.DayOfWeek)) && selectedDateTime <= DateTime.Now)
            {
                 ModelState.AddModelError("StartTime", "Please select a future time for today's recurring schedule.");
            }
            
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the view with errors
                return View(vm);
            }

            // Combine the list of enum days into a single [Flags] enum value
            RecurringWeek combinedWeek = RecurringWeek.None;
            if (vm.RecurringWeek != null && vm.RecurringWeek.Count > 0)
            {
                foreach (var day in vm.RecurringWeek)
                {
                    combinedWeek |= day; // Bitwise OR operator (e.g., Mon | Wed = 1 | 4 = 5)
                }
            }

            // 1. Map ViewModel to Schedule Model
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
            
            // --- CALCULATE END TIME (This block was correct) ---
            if (newSchedule.StartTime.HasValue && newSchedule.Duration.HasValue)
            {
                var durationTimeSpan = ScheduleHelper.GetTimeSpan(newSchedule.Duration.Value);
                newSchedule.EndTime = newSchedule.StartTime.Value.Add(durationTimeSpan);
            }
            // --- END CALCULATION ---

            _scheduleRepository.Add(newSchedule);
            return RedirectToAction("Index", "Community");
        }
        // ------------------------

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
            // --- Server-side validation for future time ---
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

            // --- FIX: ADD THIS CALCULATION BLOCK ---
            // This was missing before.
            if (newSchedule.StartTime.HasValue && newSchedule.Duration.HasValue)
            {
                // Use the new helper to get the TimeSpan
                var durationTimeSpan = ScheduleHelper.GetTimeSpan(newSchedule.Duration.Value);
                // Add it to the StartTime to get the EndTime
                newSchedule.EndTime = newSchedule.StartTime.Value.Add(durationTimeSpan);
            }
            // --- END FIX ---

            _scheduleRepository.Add(newSchedule);
            return RedirectToAction("Index", "Community");
        }
        // ------------------------

        // Competition Form
        public IActionResult CreateCompetition()
        {
            return View();
        }
                
        public IActionResult SetupMatch()
        {
            return View();
        }
    }
}