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

        [Column("schedule_type")] // Removed TypeName
        public ScheduleType? ScheduleType { get; set; }

        [Column("event_tag")] // Removed TypeName
        public EventTag? EventTag { get; set; } = Models.EventTag.None;

        [Column("description", TypeName = "longtext")]
        public string? Description { get; set; }

        [Column("location")]
        [StringLength(255)]
        public string? Location { get; set; }

        [Column("startTime")]
        public DateTime? StartTime { get; set; }

        [Column("endTime")]
        public DateTime? EndTime { get; set; }

        [Column("duration")] // Removed TypeName
        public Duration? Duration { get; set; }

        [Column("num_player")]
        public int? NumPlayer { get; set; }

        [Column("minRankRestriction", TypeName = "decimal(5,4)")]
        public decimal? MinRankRestriction { get; set; }

        [Column("maxRankRestriction", TypeName = "decimal(5,4)")]
        public decimal? MaxRankRestriction { get; set; }

        [Column("genderRestriction")] // Removed TypeName
        public GenderRestriction? GenderRestriction { get; set; } = Models.GenderRestriction.None;

        [Column("ageGroupRestriction")] // Removed TypeName
        public AgeGroupRestriction? AgeGroupRestriction { get; set; } = Models.AgeGroupRestriction.Adult;
        
        [Column("feeType")] // Removed TypeName
        public FeeType? FeeType { get; set; } = Models.FeeType.PerPerson;

        [Column("feeAmount", TypeName = "decimal(8,2)")]
        public decimal? FeeAmount { get; set; }

        [Column("privacy")] // Removed TypeName
        public Privacy? Privacy { get; set; } = Models.Privacy.Public;

        [Column("gameFeature")] // Removed TypeName
        public GameFeature? GameFeature { get; set; } = Models.GameFeature.Basic;

        [Column("cancellationfreeze")] // Removed TypeName
        public CancellationFreeze? CancellationFreeze { get; set; } = Models.CancellationFreeze.None;

        [Column("repeat")] // Removed TypeName
        public Repeat? Repeat { get; set; } = Models.Repeat.None;

        // --- NEW PROPERTIES ADDED ---
        [Column("recurringWeek")]
        public RecurringWeek? RecurringWeek { get; set; }

        [Column("autoCreateWhen")]
        public AutoCreateWhen? AutoCreateWhen { get; set; } = Models.AutoCreateWhen.B2d;
        // --- END NEW PROPERTIES ---

        [Column("hostrole")] // Removed TypeName
        public HostRole? HostRole { get; set; } = Models.HostRole.HostAndPlay;

        [Column("status")] // Removed TypeName
        public ScheduleStatus? Status { get; set; } = ScheduleStatus.Null;

        [Column("approxStartTime")]
        public DateTime? ApproxStartTime { get; set; }

        [Column("regOpen")]
        public DateTime? RegOpen { get; set; }

        [Column("regClose")]
        public DateTime? RegClose { get; set; }

        [Column("earlyBirdClose")]
        public DateTime? EarlyBirdClose { get; set; }

        // --- ADD NAVIGATION PROPERTY ---
        // This will hold the related Competition data if ScheduleType is Competition
        public virtual Competition? Competition { get; set; }
        // --- END ADD ---
    }
}