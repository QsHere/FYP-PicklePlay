using Microsoft.AspNetCore.Mvc;
using FYP_QS_CODE.Models;
using System;
using System.Collections.Generic;

namespace FYP_QS_CODE.Controllers
{
    public class CompetitionController : Controller
    {
        public IActionResult Listing()
        {
            var competitions = new List<Competition>
            {
                new Competition
                {
                    CompetitionId = 1,
                    Name = "City Championship",
                    Description = "Exciting elimination tournament for intermediate players.",
                    Location = "Downtown Arena",
                    StartTime = DateTime.Now.AddDays(7),
                    EndTime = DateTime.Now.AddDays(9),
                    Status = "Open for Registration",
                    MatchFormat = "Elimination",
                    FeeAmount = 15,
                    CommunityId = 1,
                    CreatedBy = 1
                },
                new Competition
                {
                    CompetitionId = 2,
                    Name = "Weekend Cup",
                    Description = "Fun social competition, all skill levels welcome.",
                    Location = "Riverside Club",
                    StartTime = DateTime.Now.AddDays(14),
                    EndTime = DateTime.Now.AddDays(16),
                    Status = "Registration Closed",
                    MatchFormat = "Round Robin",
                    FeeAmount = 10,
                    CommunityId = 2,
                    CreatedBy = 2
                }
            };

            return View(competitions);
        }

        public IActionResult CompDetails(int id)
        {
            // for now just return dummy view until we connect to repo
            return View();
        }
    }
}
