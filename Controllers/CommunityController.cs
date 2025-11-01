using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;
using System;
using System.Linq; // Keep this if needed elsewhere in the controller
using Microsoft.EntityFrameworkCore; // Keep this if needed elsewhere
using Microsoft.AspNetCore.Hosting; // <-- 1. ADD THIS
using System.IO; // <-- 2. ADD THIS
using System.Threading.Tasks; // <-- 3. ADD THIS

namespace FYP_QS_CODE.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // <-- 4. ADD THIS FIELD

        // 5. UPDATE THE CONSTRUCTOR to inject IWebHostEnvironment
        public CommunityController(IScheduleRepository scheduleRepository, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _scheduleRepository = scheduleRepository;
            _context = context; 
            _webHostEnvironment = webHostEnvironment; // <-- 6. ASSIGN THE FIELD
        }


        // Community Home Page
        public IActionResult Index()
        {
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
            // --- Validation ---
            if (vm.RecurringWeek == null || !vm.RecurringWeek.Any()) {
                ModelState.AddModelError("RecurringWeek", "Please select at least one day.");
            }

            if (vm.RecurringEndDate.HasValue && vm.RecurringEndDate.Value < DateTime.Today) {
                ModelState.AddModelError("RecurringEndDate", "The end date must be in the future.");
            }
            
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            // --- 1. Create the Parent "Template" Schedule ---
            
            RecurringWeek combinedWeek = RecurringWeek.None;
            foreach (var day in vm.RecurringWeek) { combinedWeek |= day; }

            var parentSchedule = new Schedule
            {
                ScheduleType = Models.ScheduleType.Recurring, 
                ParentScheduleId = null, 
                RecurringWeek = combinedWeek,
                RecurringEndDate = vm.RecurringEndDate, 
                AutoCreateWhen = vm.AutoCreateWhen,
                StartTime = DateTime.Today.Add(vm.StartTime.ToTimeSpan()), 
                EndTime = null, 
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
                HostRole = vm.HostRole,
                Status = ScheduleStatus.Active 
            };

            // --- 2. Save Parent to get its ID ---
            _scheduleRepository.Add(parentSchedule); 
            
            
            // --- 3. Generate and Save all Child "Instance" Schedules ---
            var durationTimeSpan = ScheduleHelper.GetTimeSpan(vm.Duration);
            var dayFlagMap = BuildDayFlagMap();

            for (var date = DateTime.Today; date <= vm.RecurringEndDate.Value; date = date.AddDays(1))
            {
                if (dayFlagMap.TryGetValue(date.DayOfWeek, out var dayFlag) && vm.RecurringWeek.Contains(dayFlag))
                {
                    var instanceStartTime = date.Add(vm.StartTime.ToTimeSpan());
                    
                    var instanceSchedule = new Schedule
                    {
                        ScheduleType = Models.ScheduleType.OneOff, 
                        ParentScheduleId = parentSchedule.ScheduleId, 
                        StartTime = instanceStartTime,
                        EndTime = instanceStartTime.Add(durationTimeSpan),
                        GameName = parentSchedule.GameName,
                        Description = parentSchedule.Description,
                        EventTag = parentSchedule.EventTag,
                        Location = parentSchedule.Location,
                        Duration = parentSchedule.Duration,
                        NumPlayer = parentSchedule.NumPlayer,
                        MinRankRestriction = parentSchedule.MinRankRestriction,
                        MaxRankRestriction = parentSchedule.MaxRankRestriction,
                        GenderRestriction = parentSchedule.GenderRestriction,
                        AgeGroupRestriction = parentSchedule.AgeGroupRestriction,
                        FeeType = parentSchedule.FeeType,
                        FeeAmount = parentSchedule.FeeAmount,
                        Privacy = parentSchedule.Privacy,
                        GameFeature = parentSchedule.GameFeature,
                        CancellationFreeze = parentSchedule.CancellationFreeze,
                        HostRole = parentSchedule.HostRole,
                        Status = ScheduleStatus.Active, 
                        RecurringWeek = null,
                        RecurringEndDate = null,
                        AutoCreateWhen = null
                    };
                    
                    _scheduleRepository.Add(instanceSchedule);
                }
            }

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
                HostRole = vm.HostRole,
                Status = ScheduleStatus.Active // Set status
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
        // GET Action: Shows the form
        [HttpGet] 
        public IActionResult CreateCompetition()
        {
            return View(new ScheduleCompetitionViewModel());
        }

        // --- CREATE COMPETITION ---
        // POST Action: Handles the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        // --- 7. UPDATE THE METHOD SIGNATURE ---
        public async Task<IActionResult> CreateCompetition(ScheduleCompetitionViewModel vm)
        {
            // --- Server-side validation (Keep existing date/fee checks) ---
            if (vm.EndTime <= vm.StartTime) { ModelState.AddModelError("EndTime", "End Date & Time must be after Start Date & Time."); }
            if (vm.RegClose <= vm.RegOpen) { ModelState.AddModelError("RegClose", "Registration Close Date must be after Registration Open Date."); }
            if (vm.RegClose >= vm.StartTime) { ModelState.AddModelError("RegClose", "Registration must close before the competition starts."); }
            if (vm.EarlyBirdClose.HasValue && vm.EarlyBirdClose.Value <= vm.RegOpen) { ModelState.AddModelError("EarlyBirdClose", "Early Bird Deadline must be after Registration Open Date."); }
            if (vm.EarlyBirdClose.HasValue && vm.EarlyBirdClose.Value >= vm.RegClose) { ModelState.AddModelError("EarlyBirdClose", "Early Bird Deadline must be before Registration Close Date."); }
            if (vm.FeeType == FeeType.PerPerson && !vm.FeeAmount.HasValue) { ModelState.AddModelError("FeeAmount", "Fee Amount is required for Per Team fee type."); }
            if (vm.StartTime <= DateTime.Now) { ModelState.AddModelError("StartTime", "Competition Start Date & Time must be in the future."); }


            if (!ModelState.IsValid)
            {
                return View(vm);
            }
    
            // --- 1. Handle File Upload FIRST ---
            string? uniqueImagePath = null;
            if (vm.PosterImage != null)
            {
                // 1a. Define a path to save the image
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/posters");
                Directory.CreateDirectory(uploadsFolder); // Ensures the folder exists

                // 1b. Create a unique filename to avoid conflicts
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.PosterImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 1c. Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    // --- This 'await' is what caused the error ---
                    await vm.PosterImage.CopyToAsync(fileStream);
                }

                // 1d. Store the *relative* path for the database
                uniqueImagePath = "/img/posters/" + uniqueFileName;
            }

            // 2. Map ViewModel to Schedule Model
            var newSchedule = new Schedule
            {
                ScheduleType = ScheduleType.Competition,
                GameFeature = GameFeature.Ranking,
                GameName = vm.GameName,
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
                FeeType = vm.FeeType, 
                FeeAmount = (vm.FeeType == FeeType.PerPerson) ? vm.FeeAmount : null, 
                Privacy = vm.Privacy,
                CancellationFreeze = vm.CancellationFreeze,
                HostRole = HostRole.HostOnly, 
                Status = ScheduleStatus.PendingSetup, 
                CompetitionImageUrl = uniqueImagePath // <-- Assign the new path
            };

            // 3. Create the linked Competition entity
            var newCompetition = new Competition
            {
                NumPool = 4, 
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
            var schedule = _context.Schedules
                                   .Include(s => s.Competition) 
                                   .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null) { return NotFound("Schedule not found."); }
            if (schedule.ScheduleType != ScheduleType.Competition) { return BadRequest("This schedule is not a competition."); }
            if (schedule.Competition == null) { return NotFound("Competition details missing for this schedule."); }

            var viewModel = new CompetitionSetupViewModel
            {
                ScheduleId = schedule.ScheduleId,
                GameName = schedule.GameName,
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
    if (id != vm.ScheduleId) { return BadRequest("ID mismatch."); }

    if (vm.Format == CompetitionFormat.PoolPlay)
    {
        if (vm.NumPool <= 0) { ModelState.AddModelError("NumPool", "Number of pools must be positive."); }
        if (vm.WinnersPerPool <= 0) { ModelState.AddModelError("WinnersPerPool", "Winners per pool must be positive."); }
    }

    if (!ModelState.IsValid)
    {
        return View(vm);
    }

    var scheduleToUpdate = _context.Schedules
                                 .Include(s => s.Competition)
                                 .FirstOrDefault(s => s.ScheduleId == id);

    if (scheduleToUpdate == null || scheduleToUpdate.Competition == null)
    {
        return NotFound("Competition or schedule not found for update.");
    }

    scheduleToUpdate.Competition.Format = vm.Format;

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
        scheduleToUpdate.Competition.ThirdPlaceMatch = true; 
        scheduleToUpdate.Competition.DoublePool = false;   
        scheduleToUpdate.Competition.MatchRule = null; 

    }
    else if (vm.Format == CompetitionFormat.Elimination)
    {
        scheduleToUpdate.Competition.ThirdPlaceMatch = vm.ThirdPlaceMatch;
        scheduleToUpdate.Competition.MatchRule = vm.MatchRule;
        scheduleToUpdate.Competition.NumPool = 4; 
        scheduleToUpdate.Competition.DoublePool = false; 
    }
    else if (vm.Format == CompetitionFormat.RoundRobin)
    {
        scheduleToUpdate.Competition.DoublePool = vm.DoublePool;
        scheduleToUpdate.Competition.MatchRule = vm.MatchRule;
        scheduleToUpdate.Competition.NumPool = 4; 
        scheduleToUpdate.Competition.ThirdPlaceMatch = true; 
    }

    scheduleToUpdate.Status = ScheduleStatus.Active;

    try
    {
        _context.SaveChanges(); 

        TempData["SuccessMessage"] = "Competition setup saved successfully!";
        
        // --- THIS IS THE FIX ---
        // Changed "Details", "Schedule" to "CompDetails", "Competition"
        return RedirectToAction("CompDetails", "Competition", new { id = scheduleToUpdate.ScheduleId });
    }
    catch (DbUpdateConcurrencyException)
    {
        ModelState.AddModelError("", "The record you attempted to edit was modified by another user. Please reload and try again.");
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"An error occurred saving the setup: {ex.Message}");
    }

    return View(vm);
}

        // ------------------------
        
        // POST: /Community/Publish/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Publish(int id)
        {
            var scheduleToPublish = _context.Schedules
                                          .Include(s => s.Competition) 
                                          .FirstOrDefault(s => s.ScheduleId == id);

            if (scheduleToPublish == null)
            {
                TempData["ErrorMessage"] = "Schedule not found.";
                return RedirectToAction("Index");
            }

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

            scheduleToPublish.Status = ScheduleStatus.Active;

            try
            {
                _context.SaveChanges(); 
                TempData["SuccessMessage"] = $"Competition '{scheduleToPublish.GameName}' published successfully!";
            }
            catch (Exception ex)
            {
                 TempData["ErrorMessage"] = $"Error publishing competition: {ex.Message}";
            }

            return RedirectToAction("Index"); 
        }

        // --- HELPER METHODS FOR RECURRING ---
        private RecurringWeek GetTodayDayFlag()
        {
            switch (DateTime.Today.DayOfWeek)
            {
                 case DayOfWeek.Monday:    return RecurringWeek.Mon;
                 case DayOfWeek.Tuesday:   return RecurringWeek.Tue;
                 case DayOfWeek.Wednesday: return RecurringWeek.Wed;
                 case DayOfWeek.Thursday:  return RecurringWeek.Thur;
                 case DayOfWeek.Friday:    return RecurringWeek.Fri;
                 case DayOfWeek.Saturday:  return RecurringWeek.Sat;
                 case DayOfWeek.Sunday:    return RecurringWeek.Sun;
                 default: return RecurringWeek.None;
            }
        }
        private Dictionary<DayOfWeek, RecurringWeek> BuildDayFlagMap()
        {
            return new Dictionary<DayOfWeek, RecurringWeek>
            {
                [DayOfWeek.Monday] = RecurringWeek.Mon,
                [DayOfWeek.Tuesday] = RecurringWeek.Tue,
                [DayOfWeek.Wednesday] = RecurringWeek.Wed,
                [DayOfWeek.Thursday] = RecurringWeek.Thur,
                [DayOfWeek.Friday] = RecurringWeek.Fri,
                [DayOfWeek.Saturday] = RecurringWeek.Sat,
                [DayOfWeek.Sunday] = RecurringWeek.Sun
            };
        }
    }
}