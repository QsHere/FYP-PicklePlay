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
        private readonly ApplicationDbContext _context;

        // Correct constructor for CommunityController
        public CommunityController(IScheduleRepository scheduleRepository, ApplicationDbContext context)
        {
            _scheduleRepository = scheduleRepository;
            _context = context; // Assign if needed
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

        // --- CREATE COMPETITION ---
        // GET Action: Shows the form
        [HttpGet] // <--- This attribute IS present and correct
        public IActionResult CreateCompetition()
        {
            // Pass a new, empty competition view model
            return View(new ScheduleCompetitionViewModel());
        }

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
        // --- REMOVED Format assignment ---
        // Format = vm.Format, // This will be set in SetupMatch
        // Set other defaults if needed, or rely on model defaults
        NumPool = 4, // Example default if needed here
        WinnersPerPool = 1,
        ThirdPlaceMatch = true,
        DoublePool = false,
        StandingCalculation = StandingCalculation.WinLossPoints
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


        // GET: /Community/SetupMatch/{id}
        [HttpGet]
        public IActionResult SetupMatch(int id)
        {
            // Fetch the schedule INCLUDING the competition details
            var schedule = _context.Schedules
                                   .Include(s => s.Competition) // Eager load competition data
                                   .FirstOrDefault(s => s.ScheduleId == id);

            // Basic validation
            if (schedule == null) { return NotFound("Schedule not found."); }
            if (schedule.ScheduleType != ScheduleType.Competition) { return BadRequest("This schedule is not a competition."); }
            if (schedule.Competition == null) { return NotFound("Competition details missing for this schedule."); }
            // Optional: Check if status is PendingSetup?

            // Map the fetched data to the ViewModel
            var viewModel = new CompetitionSetupViewModel
            {
                ScheduleId = schedule.ScheduleId,
                GameName = schedule.GameName,
                // --- Populate ViewModel with existing Competition data ---
                Format = schedule.Competition.Format,
                NumPool = schedule.Competition.NumPool,
                WinnersPerPool = schedule.Competition.WinnersPerPool,
                StandingCalculation = schedule.Competition.StandingCalculation,
                StandardWin = schedule.Competition.StandardWin,
                StandardLoss = schedule.Competition.StandardLoss,
                TieBreakWin = schedule.Competition.TieBreakWin,
                TieBreakLoss = schedule.Competition.TieBreakLoss,
                Draw = schedule.Competition.Draw,
                ThirdPlaceMatch = schedule.Competition.ThirdPlaceMatch,
                DoublePool = schedule.Competition.DoublePool,
                MatchRule = schedule.Competition.MatchRule
            };

            return View(viewModel);
        }

        // POST: /Community/SetupMatch/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetupMatch(int id, CompetitionSetupViewModel vm)
        {
            // Ensure ID matches
            if (id != vm.ScheduleId) { return BadRequest("ID mismatch."); }

            // Add specific validation based on format if needed
            if (vm.Format == CompetitionFormat.PoolPlay)
            {
                if (vm.NumPool <= 0) { ModelState.AddModelError("NumPool", "Number of pools must be positive."); }
                if (vm.WinnersPerPool <= 0) { ModelState.AddModelError("WinnersPerPool", "Winners per pool must be positive."); }
                // Add point validation if needed (e.g., >= 0)
            }

            if (!ModelState.IsValid)
            {
                // vm.GameName might be null if returning directly, consider reloading it if needed for display
                return View(vm);
            }

            // Fetch the existing schedule and competition again
            var scheduleToUpdate = _context.Schedules
                                         .Include(s => s.Competition)
                                         .FirstOrDefault(s => s.ScheduleId == id);

            if (scheduleToUpdate == null || scheduleToUpdate.Competition == null)
            {
                return NotFound("Competition or schedule not found for update.");
            }

            // --- Update the Competition entity with ViewModel data ---
            scheduleToUpdate.Competition.Format = vm.Format;

            // Only update fields relevant to the selected format
            if (vm.Format == CompetitionFormat.PoolPlay)
            {
                scheduleToUpdate.Competition.NumPool = vm.NumPool;
                scheduleToUpdate.Competition.WinnersPerPool = vm.WinnersPerPool;
                scheduleToUpdate.Competition.StandingCalculation = vm.StandingCalculation;
                if (vm.StandingCalculation == StandingCalculation.WinLossPoints)
                {
                    scheduleToUpdate.Competition.StandardWin = vm.StandardWin;
                    scheduleToUpdate.Competition.StandardLoss = vm.StandardLoss;
                    scheduleToUpdate.Competition.TieBreakWin = vm.TieBreakWin;
                    scheduleToUpdate.Competition.TieBreakLoss = vm.TieBreakLoss;
                    scheduleToUpdate.Competition.Draw = vm.Draw;
                }
                // Clear/Default non-Pool Play fields if desired
                scheduleToUpdate.Competition.ThirdPlaceMatch = true; // Reset to default
                scheduleToUpdate.Competition.DoublePool = false;   // Reset to default
                scheduleToUpdate.Competition.MatchRule = null; // Pool play doesn't use MatchRule per UI? Or maybe it does? Adjust if needed.

            }
            else if (vm.Format == CompetitionFormat.Elimination)
            {
                scheduleToUpdate.Competition.ThirdPlaceMatch = vm.ThirdPlaceMatch;
                scheduleToUpdate.Competition.MatchRule = vm.MatchRule;
                // Clear/Default non-Elimination fields
                scheduleToUpdate.Competition.NumPool = 4; // Reset
                                                          // ... reset points ...
                scheduleToUpdate.Competition.DoublePool = false; // Reset

            }
            else if (vm.Format == CompetitionFormat.RoundRobin)
            {
                scheduleToUpdate.Competition.DoublePool = vm.DoublePool;
                scheduleToUpdate.Competition.MatchRule = vm.MatchRule;
                // Clear/Default non-RoundRobin fields
                scheduleToUpdate.Competition.NumPool = 4; // Reset
                                                          // ... reset points ...
                scheduleToUpdate.Competition.ThirdPlaceMatch = true; // Reset
            }

            // Update Schedule status from PendingSetup to Active
            scheduleToUpdate.Status = ScheduleStatus.Active;

            try
            {
                _context.SaveChanges(); // Use DbContext to save changes since we used it to fetch
                // OR _scheduleRepository.Update(scheduleToUpdate); if repo handles attached entities correctly

                TempData["SuccessMessage"] = "Competition setup saved successfully!";
                // Redirect to Competition Details page (assuming you have one)
                // Or redirect to the main schedule details page
                return RedirectToAction("Details", "Schedule", new { id = scheduleToUpdate.ScheduleId });
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues if necessary
                ModelState.AddModelError("", "The record you attempted to edit was modified by another user. Please reload and try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred saving the setup: {ex.Message}");
            }

            // If save failed, return to view with errors
            // vm.GameName might need reloading here too
            return View(vm);
        }

        // ------------------------
        
        // POST: /Community/Publish/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Publish(int id)
        {
            var scheduleToPublish = _context.Schedules
                                          .Include(s => s.Competition) // Include competition to ensure it exists
                                          .FirstOrDefault(s => s.ScheduleId == id);

            if (scheduleToPublish == null)
            {
                TempData["ErrorMessage"] = "Schedule not found.";
                return RedirectToAction("Index");
            }

            // Validate: Is it a competition? Is it pending setup?
            if (scheduleToPublish.ScheduleType != ScheduleType.Competition || scheduleToPublish.Competition == null)
            {
                 TempData["ErrorMessage"] = "This schedule is not a competition or is missing details.";
                 return RedirectToAction("Index");
            }

            if (scheduleToPublish.Status != ScheduleStatus.PendingSetup)
            {
                 TempData["ErrorMessage"] = "This competition cannot be published because its status is not 'Pending Setup'.";
                 return RedirectToAction("Index");
            }

            // Update Status to Active
            scheduleToPublish.Status = ScheduleStatus.Active;

            try
            {
                _context.SaveChanges(); // Or use repository's update method
                TempData["SuccessMessage"] = $"Competition '{scheduleToPublish.GameName}' published successfully!";
            }
            catch (Exception ex)
            {
                 TempData["ErrorMessage"] = $"Error publishing competition: {ex.Message}";
            }

            return RedirectToAction("Index"); // Redirect back to the community index
        }
    }
}