using FYP_QS_CODE.Models;

namespace FYP_QS_CODE.Data
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> All();
        Schedule? GetById(int id);
        void Add(Schedule schedule);
        // --- ADD THESE ---
        void Update(Schedule schedule);
        void Delete(int id);
        // --- END ADD ---
    }
}