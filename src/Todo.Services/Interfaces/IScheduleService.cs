using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DomainModels;

namespace Todo.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<List<GetScheduleModel>> GetSchedulesByUserId(int scheduleId);

        Task CreateSchedule(int userId, CreateScheduleModel scheduleModel);

        Task<GetScheduleModel> GetSchedule(int userId, int scheduleId);

        Task DeleteSchedule(int userId, int scheduleId);

        Task UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel scheduleModel);
    }
}
