using Todo.DataModels;
using Todo.DomainModels;

namespace Todo.Mappers
{
    public static class ScheduleMapper
    {
        public static Schedule MapFromCreateScheduleModel(int userId, CreateScheduleModel model)
        {
            return new Schedule
            {
                Description = model.Description,
                UserId = userId,
                ScheduleDate = model.ScheduleDate
            };
        }

        public static Schedule MapFromUpdateScheduleModel(int userId, int scheduleId, UpdateScheduleModel model)
        {
            return new Schedule
            {
                ScheduleId = scheduleId,
                Description = model.Description,
                UserId = userId,
                ScheduleDate = model.ScheduleDate
            };
        }

        public static GetScheduleModel MapToGetScheduleModel(Schedule item)
        {
            return new GetScheduleModel
            {
                ScheduleId = item.ScheduleId,
                Description = item.Description,
                UserId = item.UserId,
                ScheduleDate = item.ScheduleDate
            };
        }
    }
}
