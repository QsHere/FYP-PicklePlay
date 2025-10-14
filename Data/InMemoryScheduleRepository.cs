using System;
using System.Collections.Generic;
using System.Linq;
using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Data
{
    public class InMemoryScheduleRepository : IScheduleRepository
    {
        private readonly List<Schedule> _schedules = new();

        public IEnumerable<Schedule> All() => _schedules.OrderByDescending(s => s.StartTime);

        public Schedule? Get(Guid id) => _schedules.FirstOrDefault(s => s.Id == id);

        public void Add(Schedule s) => _schedules.Add(s);

        public void Update(Schedule s)
        {
            var i = _schedules.FindIndex(x => x.Id == s.Id);
            if (i >= 0) _schedules[i] = s;
        }

        public void AddJoinRequest(Guid scheduleId, JoinRequest req)
        {
            var s = Get(scheduleId);
            if (s == null) return;
            s.JoinRequests.Add(req);
        }

        public void UpdateJoinRequest(Guid scheduleId, Guid requestId, JoinStatus status)
        {
            var s = Get(scheduleId);
            if (s == null) return;
            var r = s.JoinRequests.FirstOrDefault(x => x.Id == requestId);
            if (r == null) return;
            r.Status = status;
        }

        public void ConfirmRequest(Guid scheduleId, Guid requestId)
        {
            var s = Get(scheduleId);
            if (s == null) return;
            var r = s.JoinRequests.FirstOrDefault(x => x.Id == requestId);
            if (r == null) return;

            r.Status = JoinStatus.Confirmed;
            s.Participants.Add(new Participant { DisplayName = r.RequesterName, Paid = false });
        }

        public void Seed()
        {
            if (_schedules.Any()) return;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // ✅ Today Morning Game
            _schedules.Add(new Schedule
            {
                Title = "Morning Social Doubles",
                Description = "Casual morning games for intermediate players.",
                GameType = GameType.Social,
                Location = "Court 1, City Sports Hall",
                StartTime = today.AddHours(9),
                EndTime = today.AddHours(11),
                CourtCount = 2,
                ParticipantLimit = 16,
                CommunityName = "City Pickleballers",
                Restriction = "Rank 3.0 – 4.0",
                PricePerPlayer = 10,
                Tags = new() { EventTag.Competitive },
                Participants = new()
                {
                    new Participant{ DisplayName="Alice", IsOrganizer=true, ProfilePicUrl="/img/avatar1.png", Paid=true },
                    new Participant{ DisplayName="Ben", ProfilePicUrl="/img/avatar2.png", Paid=true },
                    new Participant{ DisplayName="Chris", ProfilePicUrl="/img/avatar3.png", Paid=true }
                },
                IsBookmarked = true,
                IsCancelled = false,
                IsQuit = false,
                IsEnded = false  // ✅ game still active until host clicks End Game
            });

            // ✅ Today Afternoon Game
            _schedules.Add(new Schedule
            {
                Title = "Afternoon Beginner Session",
                Description = "Beginner-friendly social match with coaching tips.",
                GameType = GameType.Social,
                Location = "Court 3, Riverside Sports Complex",
                StartTime = today.AddHours(14),
                EndTime = today.AddHours(16),
                CourtCount = 3,
                ParticipantLimit = 20,
                CommunityName = "Riverside Pickleball Club",
                Restriction = "Beginner – Intermediate",
                PricePerPlayer = 5,
                Tags = new() { EventTag.BeginnerFriendly },
                Participants = new()
                {
                    new Participant{ DisplayName="Dina", IsOrganizer=true, ProfilePicUrl="/img/avatar4.png", Paid=true },
                    new Participant{ DisplayName="Ethan", ProfilePicUrl="/img/avatar5.png", Paid=true }
                },
                IsBookmarked = true,
                IsCancelled = false,
                IsQuit = false,
                IsEnded = false  // ✅ game still active until host clicks End Game
            });

            // ✅ Today Evening Game
            _schedules.Add(new Schedule
            {
                Title = "Evening Social Round Robin",
                Description = "Fun and friendly round robin for all levels.",
                GameType = GameType.Social,
                Location = "Court 5, Downtown Club",
                StartTime = DateTime.Today.AddDays(-2).AddHours(10),
                EndTime = DateTime.Today.AddDays(-2).AddHours(12),
                CourtCount = 4,
                ParticipantLimit = 24,
                CommunityName = "Downtown Pickle Club",
                Restriction = "Open to All",
                PricePerPlayer = 8,
                Tags = new() { EventTag.BeginnerFriendly, EventTag.Training },
                Participants = new()
                {
                    new Participant{ DisplayName="Fiona", IsOrganizer=true, ProfilePicUrl="/img/avatar6.png", Paid=true },
                    new Participant{ DisplayName="George", ProfilePicUrl="/img/avatar7.png", Paid=true },
                    new Participant{ DisplayName="Hana", ProfilePicUrl="/img/avatar8.png", Paid=true }
                },
                IsBookmarked = true,
                IsCancelled = false,
                IsQuit = false,
                IsEnded = true,  // ✅ game still active until host clicks End Game
                Endorsements = new()
   {
        new Endorsement { ParticipantName = "Alice", Skill = "Volley", Personality = "Leadership" },
        new Endorsement { ParticipantName = "Ben", Skill = "Dink", Personality = "Friendly" },
    }
            });

            // ✅ Tomorrow Morning Game
            _schedules.Add(new Schedule
            {
                Title = "Morning Ladder Matches",
                Description = "Climb the ladder system! Competitive but social.",
                GameType = GameType.Social,
                Location = "Court 2, Green Valley Club",
                StartTime = tomorrow.AddHours(8),
                EndTime = tomorrow.AddHours(10),
                CourtCount = 3,
                ParticipantLimit = 12,
                CommunityName = "Green Valley Pickleball",
                Restriction = "Rank 3.5 – 4.5",
                PricePerPlayer = 12,
                Tags = new() { EventTag.SingleGame },
                Participants = new()
                {
                    new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true }
                },
                IsBookmarked = true,
                IsCancelled = false,
                IsQuit = true,
                IsEnded = false  // ✅ game still active until host clicks End Game
            });
            _schedules.Add(new Schedule
            {
                Title = "Last Weekend Match",
                Description = "This game has already finished.",
                GameType = GameType.Social,
                Location = "Court 9, Old Arena",
                StartTime = DateTime.Today.AddDays(-2).AddHours(10),
                EndTime = DateTime.Today.AddDays(-2).AddHours(12),
                CourtCount = 2,
                ParticipantLimit = 12,
                CommunityName = "Old Club",
                Restriction = "Open",
                PricePerPlayer = 5,
                Tags = new() { EventTag.Competitive },
                Participants = new()
    {
        new Participant { DisplayName="Old Host", IsOrganizer=true, ProfilePicUrl="/img/avatarOld.png", Paid=true },
                           new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true },
                                       new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true },
                                       new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true },
                                       new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true },
                                       new Participant{ DisplayName="Ian", IsOrganizer=true, ProfilePicUrl="/img/avatar9.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true },
                    new Participant{ DisplayName="Jenny", ProfilePicUrl="/img/avatar10.png", Paid=true }
    },
                IsBookmarked = false,
                IsCancelled = false,
                IsQuit = false,
                IsEnded = true  // ✅ game still active until host clicks End Game
            });
            // ✅ Tomorrow Evening Game
            _schedules.Add(new Schedule
            {
                Title = "Weekend Night Social",
                Description = "Casual night game to wrap up the weekend.",
                GameType = GameType.Social,
                Location = "Court 4, Lakeside Arena",
                StartTime = DateTime.Today.AddDays(-2).AddHours(10),
                EndTime = DateTime.Today.AddDays(-2).AddHours(12),
                CourtCount = 2,
                ParticipantLimit = 14,
                CommunityName = "Lakeside Pickleballers",
                CommunityIconUrl = "/img/community1.png",
                Restriction = "Open to All",
                PricePerPlayer = 7,
                Tags = new() { EventTag.Competitive },
                Participants = new()
                {
                    new Participant{ DisplayName="Kevin", IsOrganizer=true, ProfilePicUrl="/img/avatar11.png", Paid=true },
                    new Participant{ DisplayName="Linda", ProfilePicUrl="/img/avatar12.png", Paid=true },
                    new Participant{ DisplayName="Mark", ProfilePicUrl="/img/avatar13.png", Paid=true }
                },
                IsBookmarked = true,
                IsCancelled = true,
                IsQuit = false,
                IsEnded = true,  // ✅ game still active until host clicks End Game

                Endorsements = new()
    {
        new Endorsement { ParticipantName = "Alice", Skill = "Volley", Personality = "Leadership" },
        new Endorsement { ParticipantName = "Ben", Skill = "Dink", Personality = "Friendly" }
    }
            });
        }
    }
}