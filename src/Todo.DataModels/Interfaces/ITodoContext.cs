using System.Collections.Generic;

namespace Todo.DataModels.Interfaces
{
    public interface ITodoContext
    {
        List<User> Users { get; set; }

        List<Schedule> Schedules { get; set; }

        int NextUserId { get; }

        int NextScheduleId { get; }
    }
}
