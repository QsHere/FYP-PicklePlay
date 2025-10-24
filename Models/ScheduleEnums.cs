using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FYP_QS_CODE.Models
{
    // Enums based on your CREATE TABLE statement
    
    public enum ScheduleType
    {
        OneOff,
        Recurring,
        Competition
    }

    public enum EventTag
    {
        None,
        [Display(Name = "Beginner-Friendly")]
        BeginnerFriendly,
        Competitive,
        [Display(Name = "Single-Game")]
        SingleGame,
        Training
    }

    public enum Duration
    {
        [Display(Name = "0.5 hr")] [EnumMember(Value = "0.5h")] H0_5,
        [Display(Name = "1 hr")] [EnumMember(Value = "1h")] H1,
        [Display(Name = "1.5 hr")] [EnumMember(Value = "1.5h")] H1_5,
        [Display(Name = "2 hr")] [EnumMember(Value = "2h")] H2,
        [Display(Name = "2.5 hr")] [EnumMember(Value = "2.5h")] H2_5,
        [Display(Name = "3 hr")] [EnumMember(Value = "3h")] H3,
        [Display(Name = "3.5 hr")] [EnumMember(Value = "3.5h")] H3_5,
        [Display(Name = "4 hr")] [EnumMember(Value = "4h")] H4,
        [Display(Name = "5 hr")] [EnumMember(Value = "5h")] H5,
        [Display(Name = "6 hr")] [EnumMember(Value = "6h")] H6,
        [Display(Name = "7 hr")] [EnumMember(Value = "7h")] H7,
        [Display(Name = "8 hr")] [EnumMember(Value = "8h")] H8,
        [Display(Name = "1 day")] [EnumMember(Value = "1d")] D1,
        [Display(Name = "2 days")] [EnumMember(Value = "2d")] D2,
        [Display(Name = "3 days")] [EnumMember(Value = "3d")] D3
    }

    public enum GenderRestriction
    {
        None,
        Male,
        Female
    }

    public enum AgeGroupRestriction
    {
        Junior, // (Under 18)
        Adult,  // (18-55)
        Senior  // (Above 55)
    }

    public enum FeeType
    {
        None,
        Free,
        [Display(Name = "Auto Split Total")]
        AutoSplitTotal,
        [Display(Name = "Per Person")]
        PerPerson
    }

    public enum Privacy
    {
        Public,
        Private
    }

    public enum GameFeature
    {
        Basic,
        Ranking
    }

    public enum CancellationFreeze
    {
        None,
        [Display(Name = "2 hr before")] [EnumMember(Value = "2hr before")] B2hr,
        [Display(Name = "4 hr before")] [EnumMember(Value = "4hr before")] B4hr,
        [Display(Name = "6 hr before")] [EnumMember(Value = "6hr before")] B6hr,
        [Display(Name = "8 hr before")] [EnumMember(Value = "8hr before")] B8hr,
        [Display(Name = "12 hr before")] [EnumMember(Value = "12hr before")] B12hr,
        [Display(Name = "24 hr before")] [EnumMember(Value = "24hr before")] B24hr
    }

    public enum Repeat
    {
        None,
        [Display(Name = "Repeat for 1 week")] [EnumMember(Value = "Repeat for 1 week")] W1,
        [Display(Name = "Repeat for 2 weeks")] [EnumMember(Value = "Repeat for 2 week")] W2, // Note: DB schema has typo "2 week"
        [Display(Name = "Repeat for 3 weeks")] [EnumMember(Value = "Repeat for 3 week")] W3, // Note: DB schema has typo "3 week"
        [Display(Name = "Repeat for 4 weeks")] [EnumMember(Value = "Repeat for 4 week")] W4  // Note: DB schema has typo "4 week"
    }

    public enum HostRole
    {
        [Display(Name = "Host & Play")]
        HostAndPlay,
        [Display(Name = "Host Only")]
        HostOnly
    }

    public enum ScheduleStatus
    {
        Null,
        Active,
        Past,
        Quit,
        Cancelled
    }
}