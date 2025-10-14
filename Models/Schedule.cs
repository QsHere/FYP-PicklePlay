using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP_QS_CODE.Models
{
    public enum GameType { Social = 0, Competition = 1 }
    public enum CompetitionFormat { Knockout = 0, RoundRobin = 1, PoolPlay = 2 }
    public enum EventTag { BeginnerFriendly = 0, Competitive = 1, Training = 2, SingleGame = 3, Social = 4}

    public class Schedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(80)]
        public string Title { get; set; } = "";

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public GameType GameType { get; set; }

        public CompetitionFormat? CompetitionFormat { get; set; }

        [Required, StringLength(80)]
        public string Location { get; set; } = "";   // 🔹 Changed from Venue → Location

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Range(1, 16)]
        [Display(Name = "Courts")]
        public int CourtCount { get; set; } = 1;

        [Range(2, 200)]
        [Display(Name = "Participant Limit")]
        public int ParticipantLimit { get; set; } = 8;

        public List<EventTag> Tags { get; set; } = new();

        // NEW 🔹
        public string CommunityName { get; set; } = "PicklePlay Club";
        public string CommunityIconUrl { get; set; } = string.Empty;

        public string Restriction { get; set; } = "Open to All";
        public decimal PricePerPlayer { get; set; } = 0;

        public List<Participant> Participants { get; set; } = new();
        public List<JoinRequest> JoinRequests { get; set; } = new();
        public List<BracketMatch> Bracket { get; set; } = new();

        // Properties for MyGames page
        public bool IsCancelled { get; set; }   // mark if game cancelled
        public bool IsQuit { get; set; }        // mark if user quit
        public bool IsBookmarked { get; set; }  // mark if user bookmarked
        public bool IsEnded { get; set; } // true only when host clicks "End Game"

        
public List<Endorsement> Endorsements { get; set; } = new();
    }

    public class Participant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, StringLength(60)]
        public string DisplayName { get; set; } = "";
        public bool IsOrganizer { get; set; } = false;   // 🔹 New
        public bool Paid { get; set; }
        public string ProfilePicUrl { get; set; } = "/img/default-avatar.png";  // 🔹 New
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }

    public enum JoinStatus { Requested, Confirmed, OnHold, Declined }

    public class JoinRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RequesterName { get; set; } = "";
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public JoinStatus Status { get; set; } = JoinStatus.Requested;
    }

    public enum MatchStatus { NotStarted, InProgress, Final }

    public class BracketMatch
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Round { get; set; }
        public string SideA { get; set; } = "-";
        public string SideB { get; set; } = "-";
        public string Score { get; set; } = "";
        public MatchStatus Status { get; set; } = MatchStatus.NotStarted;
    }
    
    
}
