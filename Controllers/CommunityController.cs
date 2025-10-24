using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;
using System; // Add this

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

        // Recurring Schedule Form
        public IActionResult CreateRecurring()
        {
            return View();
        }

        // --- CREATE ONE-OFF ---
        // GET Action: Shows the form
        [HttpGet]
        public IActionResult CreateOneOff()
        {
            // Pass a new, empty view model to the view
            return View(new ScheduleCreateViewModel());
        }

        // POST Action: Handles the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOneOff(ScheduleCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails (e.g., required field missing),
                // return to the view with the user's data to show errors.
                return View(vm);
            }

            // 1. Map ViewModel (from form) to Schedule (for database)
            var newSchedule = new Schedule
            {
                // This is a One-Off schedule
                ScheduleType = Models.ScheduleType.OneOff, 
                
                // Map all other fields from the view model
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
                // Only set FeeAmount if the type is not None or Free
                FeeAmount = (vm.FeeType == FeeType.AutoSplitTotal || vm.FeeType == FeeType.PerPerson) ? vm.FeeAmount : null,
                Privacy = vm.Privacy,
                GameFeature = vm.GameFeature,
                CancellationFreeze = vm.CancellationFreeze,
                Repeat = vm.Repeat,
                HostRole = vm.HostRole
            };

            // 2. Add to database via the repository
            _scheduleRepository.Add(newSchedule);

            // 3. Redirect to a success page (e.g., the main community page)
            // You can change this to redirect to a "Details" page later
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