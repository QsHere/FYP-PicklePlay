using System;
using System.Collections.Generic;
using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Data
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> All();
        Schedule? Get(Guid id);
        void Add(Schedule s);
        void Update(Schedule s);
        void AddJoinRequest(Guid scheduleId, JoinRequest req);
        void UpdateJoinRequest(Guid scheduleId, Guid requestId, JoinStatus status);
        void ConfirmRequest(Guid scheduleId, Guid requestId);
        void Seed();
    }
}
