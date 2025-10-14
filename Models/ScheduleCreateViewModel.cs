using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP_QS_CODE.Models
{
    public class ScheduleCreateViewModel
    {
        [Required, StringLength(80)]
        public string Title { get; set; } = "";

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public GameType GameType { get; set; } = GameType.Social;

        public CompetitionFormat? CompetitionFormat { get; set; }

        [Required, StringLength(80)]
        public string Location { get; set; } = "";

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.Today.AddHours(18);

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; } = DateTime.Today.AddHours(20);

        [Range(1, 16)]
        [Display(Name = "Courts")]
        public int CourtCount { get; set; } = 2;

        [Range(2, 200)]
        [Display(Name = "Participant Limit")]
        public int ParticipantLimit { get; set; } = 16;

        public List<EventTag> Tags { get; set; } = new();
    }
}
