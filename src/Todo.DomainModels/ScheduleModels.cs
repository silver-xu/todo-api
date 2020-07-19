using System;

namespace Todo.DomainModels
{
    public abstract class BaseScheduleModel
    {
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }

    }

    public class CreateScheduleModel : BaseScheduleModel
    {
        public int UserId { get; set; }
    }

    public class UpdateScheduleModel : BaseScheduleModel
    {
    }

    public class GetScheduleModel : BaseScheduleModel
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
    }
}
