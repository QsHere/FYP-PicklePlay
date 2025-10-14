using System;

namespace FYP_QS_CODE.Models
{
    public class Endorsement
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ParticipantName { get; set; } = string.Empty;

        // Only 1 skill per participant
        public string Skill { get; set; } = string.Empty;

        // Only 1 personality per participant
        public string Personality { get; set; } = string.Empty;

        public DateTime GivenAt { get; set; } = DateTime.Now;
    }
}
