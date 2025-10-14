using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Data;
using FYP_QS_CODE.Models;

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

        // One-Off Game Form
        public IActionResult CreateOneOff()
        {
            return View();
        }

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
