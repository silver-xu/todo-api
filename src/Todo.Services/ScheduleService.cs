using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DomainModels;
using Todo.DomainModels.Exceptions;
using Todo.Mappers;
using Todo.Repositories.Interfaces;
using Todo.Services.Interfaces;

namespace Todo.Services
{
    public class ScheduleService : IScheduleService
    {
        private IScheduleRepository _scheduleRepository
            ;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task CreateSchedule(int userId, CreateScheduleModel scheduleModel)
        {
            var schedule = ScheduleMapper.MapFromCreateScheduleModel(userId, scheduleModel);

            await _scheduleRepository.AddSchedule(schedule);
        }

        public async Task DeleteSchedule(int userId, int scheduleId)
        {
            var schedule = await _scheduleRepository.GetSchedule(scheduleId);
            if (schedule == null || schedule.UserId != userId)
            {
                throw new NotFoundException();
            }

            await _scheduleRepository.DeleteSchedule(scheduleId);
        }

        public async Task<List<GetScheduleModel>> GetSchedulesByUserId(int userId)
        {
            var schedules = await _scheduleRepository.QuerySchedules(item => item.UserId == userId);

            return schedules.Select(item => ScheduleMapper.MapToGetScheduleModel(item)).ToList();
        }

        public async Task<GetScheduleModel> GetSchedule(int userId, int scheduleId)
        {
            var schedule = await _scheduleRepository.GetSchedule(scheduleId);

            if (schedule == null || schedule.UserId != userId)
            {
                throw new NotFoundException();
            }

            return ScheduleMapper.MapToGetScheduleModel(schedule);
        }

        public async Task UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel scheduleModel)
        {
            var schedule = await _scheduleRepository.GetSchedule(scheduleId);
            if (schedule == null || schedule.UserId != userId)
            {
                throw new NotFoundException();
            }

            var updatedSchedule = ScheduleMapper.MapFromUpdateScheduleModel(userId, scheduleId, scheduleModel);

            await _scheduleRepository.UpdateSchedule(updatedSchedule);
        }
    }

}
