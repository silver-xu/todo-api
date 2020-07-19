using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DataModels;
using Todo.DataModels.Interfaces;
using Todo.Repositories.Interfaces;

namespace Todo.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private ITodoContext _todoContext;

        public ScheduleRepository(ITodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task AddSchedule(Schedule task)
        {
            await Task.Run(() =>
            {
                task.ScheduleId = _todoContext.NextScheduleId;
                _todoContext.Schedules.Add(task);
            });
        }

        public async Task DeleteSchedule(int scheduleId)
        {
            await Task.Run(() =>
                _todoContext.Schedules.RemoveAll(schedule => schedule.ScheduleId == scheduleId)
            );
        }

        public async Task<Schedule> GetSchedule(int scheduleId)
        {
            return await Task.Run(() =>
                _todoContext.Schedules.FirstOrDefault(schedule => schedule.ScheduleId == scheduleId)
            );
        }


        public async Task UpdateSchedule(Schedule schedule)
        {
            await Task.Run(() =>
            {
                var scheduleToUpdate = _todoContext.Schedules.FirstOrDefault(s => s.ScheduleId == schedule.ScheduleId);

                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.UserId = schedule.UserId;
                    scheduleToUpdate.Description = schedule.Description;
                    scheduleToUpdate.ScheduleDate = schedule.ScheduleDate;
                }
            });
        }

        public async Task<List<Schedule>> ListSchedules()
        {
            return await Task.Run(() => _todoContext.Schedules.ToList());
        }

        public async Task<List<Schedule>> QuerySchedules(Func<Schedule, bool> predicate)
        {
            return await Task.Run(() =>
            {
                return _todoContext.Schedules.Where(predicate).ToList();
            });
        }

    }
}
