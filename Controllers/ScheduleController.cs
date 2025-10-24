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

        // This is your main schedule listing page
        public IActionResult Index()
        {
            var items = _repo.All();
            return View(items);
        }

        public IActionResult CompSchedule()
        {
            return View();
        }

        // MyGames can now show all statuses (including cancelled, past, etc.)
        public IActionResult MyGames()
        {
            // We use .ToList() to get all data from DB first.
            var schedules = _repo.All().ToList(); 
            return View(schedules);
        }

        // Changed from Guid id to int id
        public IActionResult Details(int id)
        {
            var s = _repo.Get(id);
            if (s == null) return NotFound();
            return View(s);
        }

        // Changed from Guid id to int id
        public IActionResult Edit(int id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();
            return View(game); // Returns Views/Schedule/Edit.cshtml (which you'll need to create)
        }
        
        // TODO: You will need to create an [HttpPost] Edit action 
        // similar to the CreateOneOff post action to save changes.

        // Changed from Guid id to int id
        // Updated logic to use the new 'Status' enum
        public IActionResult Cancel(int id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            // Updated logic for new database schema
            game.Status = ScheduleStatus.Cancelled; 
            
            _repo.Update(game);
            return RedirectToAction("MyGames");
        }

        // --- All 'Create' and 'JoinRequest' actions have been removed ---
        // 'Create' is now in CommunityController.
        // 'JoinRequest' functionality will be rebuilt later with new tables.
    }
}