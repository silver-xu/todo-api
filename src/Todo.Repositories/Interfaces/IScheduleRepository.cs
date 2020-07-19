using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataModels;

namespace Todo.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task AddSchedule(Schedule schedule);
        Task UpdateSchedule(Schedule schedule);
        Task DeleteSchedule(int scheduleId);
        Task<Schedule> GetSchedule(int scheduleId);
        Task<List<Schedule>> ListSchedules();
        Task<List<Schedule>> QuerySchedules(Func<Schedule, bool> predicate);
    }
}
