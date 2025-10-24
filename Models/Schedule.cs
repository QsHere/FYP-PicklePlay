using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYP_QS_CODE.Models
{
    [Table("schedule")]
    public class Schedule
    {
        [Key]
        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("gameName")]
        [StringLength(255)]
        public string? GameName { get; set; }

        [Column("schedule_type", TypeName = "enum('OneOff','Recurring','Competition')")]
        public ScheduleType? ScheduleType { get; set; }

        [Column("event_tag", TypeName = "enum('None','Beginner-Friendly','Competitive','Single-Game','Training')")]
        public EventTag? EventTag { get; set; } = Models.EventTag.None;

        [Column("description", TypeName = "longtext")]
        public string? Description { get; set; }

        [Column("location")]
        [StringLength(255)]
        public string? Location { get; set; }

        [Column("startTime")]
        public DateTime? StartTime { get; set; }

        [Column("duration", TypeName = "enum('0.5h','1h','1.5h','2h','2.5h','3h','3.5h','4h','5h','6h','7h','8h','1d','2d','3d')")]
        public Duration? Duration { get; set; }

        [Column("num_player")]
        public int? NumPlayer { get; set; }

        [Column("minRankRestriction", TypeName = "decimal(5,4)")]
        public decimal? MinRankRestriction { get; set; }

        [Column("maxRankRestriction", TypeName = "decimal(5,4)")]
        public decimal? MaxRankRestriction { get; set; }

        [Column("genderRestriction", TypeName = "enum('None','Male','Female')")]
        public GenderRestriction? GenderRestriction { get; set; } = Models.GenderRestriction.None;

        [Column("ageGroupRestriction", TypeName = "enum('Junior','Adult','Senior')")]
        public AgeGroupRestriction? AgeGroupRestriction { get; set; } = Models.AgeGroupRestriction.Adult;
        
        [Column("feeType", TypeName = "enum('None','Free','AutoSplitTotal','PerPerson')")]
        public FeeType? FeeType { get; set; } = Models.FeeType.PerPerson;

        [Column("feeAmount", TypeName = "decimal(8,2)")]
        public decimal? FeeAmount { get; set; }

        [Column("privacy", TypeName = "enum('Public','Private')")]
        public Privacy? Privacy { get; set; } = Models.Privacy.Public;

        [Column("gameFeature", TypeName = "enum('Basic','Ranking')")]
        public GameFeature? GameFeature { get; set; } = Models.GameFeature.Basic;

        [Column("cancellationfreeze", TypeName = "enum('None','2hr before','4hr before','6hr before','8hr before','12hr before','24hr before')")]
        public CancellationFreeze? CancellationFreeze { get; set; } = Models.CancellationFreeze.None;

        [Column("repeat", TypeName = "enum('None','Repeat for 1 week','Repeat for 2 week','Repeat for 3 week','Repeat for 4 week')")]
        public Repeat? Repeat { get; set; } = Models.Repeat.None;

        [Column("hostrole", TypeName = "enum('HostAndPlay','HostOnly')")]
        public HostRole? HostRole { get; set; } = Models.HostRole.HostAndPlay;

        // --- Other columns from your table ---
        // We are skipping these as requested or they have defaults:
        // schedule_id (auto-increment)
        // approxStartTime (for competition)
        // endTime (for competition)
        // num_team (for competition)
        // status (has default)
        // recurringWeek (for recurring)
        // autoCreateWhen (has default)

        [Column("status", TypeName = "enum('Null','Active','Past','Quit','Cancelled')")]
        public ScheduleStatus? Status { get; set; } = ScheduleStatus.Null;
    }
}