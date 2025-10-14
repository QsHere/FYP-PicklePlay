using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleRepository _repo;

        public ScheduleController(IScheduleRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var items = _repo.All();
            return View(items);
        }

         public IActionResult CompSchedule()
        {
            return View();
        }

public IActionResult MyGames()
{
    var schedules = _repo.All().ToList(); // ensure it's not null
    return View(schedules);
}

        public IActionResult Details(Guid id)
        {
            var s = _repo.Get(id);
            if (s == null) return NotFound();
            return View(s);
        }

public IActionResult Edit(Guid id)
{
    var game = _repo.Get(id);
    if (game == null) return NotFound();
    return View(game); // create/Edit.cshtml
}

public IActionResult Cancel(Guid id)
{
    var game = _repo.Get(id);
    if (game == null) return NotFound();
    game.IsCancelled = true;
    _repo.Update(game);
    return RedirectToAction("MyGames");
}
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ScheduleCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ScheduleCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var schedule = new Schedule
            {
                Title = vm.Title,
                Description = vm.Description,
                GameType = vm.GameType,
                CompetitionFormat = vm.GameType == GameType.Competition ? vm.CompetitionFormat : null,
                Location = vm.Location,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                CourtCount = vm.CourtCount,
                ParticipantLimit = vm.ParticipantLimit,
                Tags = vm.Tags?.ToList() ?? new()
            };

            // rudimentary auto-bracket if competition
            if (schedule.GameType == GameType.Competition)
            {
                var repo = _repo as InMemoryScheduleRepository;
                // create 8-slot sample bracket by default
                schedule.Bracket = new InMemoryScheduleRepository().GetType()
                    .GetMethod("GenerateKnockoutBracket", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .Invoke(null, new object[] { 8 }) as System.Collections.Generic.List<BracketMatch> ?? new();
            }

            _repo.Add(schedule);
            return RedirectToAction(nameof(Details), new { id = schedule.Id });
        }

        [HttpGet]
        public IActionResult ManageRequests(Guid id)
        {
            var s = _repo.Get(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestJoin(Guid id, string requesterName)
        {
            if (string.IsNullOrWhiteSpace(requesterName)) requesterName = "Anonymous";
            _repo.AddJoinRequest(id, new JoinRequest { RequesterName = requesterName });
            return RedirectToAction(nameof(ManageRequests), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Accept(Guid id, Guid requestId)
        {
            _repo.ConfirmRequest(id, requestId);
            return RedirectToAction(nameof(ManageRequests), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decline(Guid id, Guid requestId)
        {
            _repo.UpdateJoinRequest(id, requestId, JoinStatus.Declined);
            return RedirectToAction(nameof(ManageRequests), new { id });
        }
    }
}
