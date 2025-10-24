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
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the view with errors
                return View(vm);
            }

            // --- NEW LOGIC FOR MULTI-SELECT ---
            // Combine the list of enum days into a single [Flags] enum value
            RecurringWeek combinedWeek = RecurringWeek.None;
            if (vm.RecurringWeek != null && vm.RecurringWeek.Count > 0)
            {
                foreach (var day in vm.RecurringWeek)
                {
                    combinedWeek |= day; // Bitwise OR operator (e.g., Mon | Wed = 1 | 4 = 5)
                }
            }
            // --- END NEW LOGIC ---

            // 1. Map ViewModel to Schedule Model
            var newSchedule = new Schedule
            {
                // This is a Recurring schedule
                ScheduleType = Models.ScheduleType.Recurring, 
                
                // Map recurring-specific fields
                RecurringWeek = combinedWeek, // Assign the combined value
                AutoCreateWhen = vm.AutoCreateWhen,
                // Combine 'Today' with the Time from the form to create a valid DateTime
                StartTime = DateTime.Today.Add(vm.StartTime.ToTimeSpan()), 
                
                // Map all other common fields
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
                // 'Repeat' is intentionally null, as this is not a one-off
            };

            // 2. Add to database
            _scheduleRepository.Add(newSchedule);

            // 3. Redirect to the main page
            return RedirectToAction("Index", "Community");
        }
        // ------------------------

        // --- CREATE ONE-OFF --- (This is unchanged)
        [HttpGet]
        public IActionResult CreateOneOff()
        {
            return View(new ScheduleCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOneOff(ScheduleCreateViewModel vm)
        {
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