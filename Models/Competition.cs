using System;
using System.Collections.Generic;

namespace FYP_QS_CODE.Models
{
    public class Competition
    {
        public int CompetitionId { get; set; }

        // Basic Info
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }

        // Time & Schedule
        public DateTime ApproximateStartingTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? EarlyBirdDeadline { get; set; }
        public int DurationDays { get; set; }

        // Restrictions
        public int? MinRankRestriction { get; set; }
        public int? MaxRankRestriction { get; set; }
        public string? GenderRestriction { get; set; }  // e.g. Male / Female / Mixed
        public string? AgeGroupRestriction { get; set; } // e.g. U18 / Adult

        // Fee
        public string GameFeeType { get; set; } = "Free"; // Free / Paid
        public decimal FeeAmount { get; set; } = 0;

        // Privacy & Status
        public string Privacy { get; set; } = "public"; // public / private
        public string Status { get; set; } = "registration"; // registration / active / hidden(cancel/quit) / past

        // Community & Creator
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public int CreatedBy { get; set; } // FK → User
        public User? Creator { get; set; }
        public string? MatchFormat { get; set; } // Pool Play / Elimination / Round Robin

        // Related Entities
        public MatchSetup? MatchSetup { get; set; }
        public List<User> Staff { get; set; } = new();
        public List<Team> ConfirmedTeams { get; set; } = new();
        public List<Team> PendingTeams { get; set; } = new();
        public List<Team> TeamLimit { get; set; } = new();
        public List<User> Spectators { get; set; } = new();
         
    }
    

    public class MatchSetup
    {
        public int MatchSetupId { get; set; }
        public int CompetitionId { get; set; }

        public string Format { get; set; } = "Pool Play"; // Pool Play / Elimination / Round Robin
        public string? OptionsJson { get; set; } // store JSON rules e.g. pool size, scoring rules
    }

    // Simplified Team & User for now (can expand later)
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int TeamLimit { get; set; } = new();
        public List<User> Members { get; set; } = new();
    }

    public class User
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? ProfilePicUrl { get; set; }
    }

    public class Community
    {
        public int CommunityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
    }
}
