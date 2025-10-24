using System;
using System.Collections.Generic;
using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Data
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> All();
        Schedule? Get(int id); // Changed from Guid to int
        void Add(Schedule s);
        void Update(Schedule s);
        
        // We remove JoinRequest/ConfirmRequest/Seed
        // They will be re-added when you build those features
        // with their own tables and repositories.
    }
}