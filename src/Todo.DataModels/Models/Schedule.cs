using System;

namespace Todo.DataModels
{
    public partial class Schedule
    {
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
