using System;
using System.Collections.Generic;
using Todo.DataModels.Interfaces;

namespace Todo.DataModels
{
    public class InMemoryTodoContext : ITodoContext
    {
        public InMemoryTodoContext()
        {
            Users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Token = "2b8a21c1-16b1-48f8-bc31-6d177347f23d"
                },
                new User
                {
                    UserId = 2,
                    Token = "79f7695e-da40-42cf-a02f-ab4537ed367b"
                }
            };

            Schedules = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    UserId = 1,
                    Description = "Foo",
                    ScheduleDate = DateTime.Now.AddDays(1)
                },
                new Schedule
                {
                    ScheduleId = 2,
                    UserId = 1,
                    Description = "Bar",
                    ScheduleDate = DateTime.Now.AddDays(1)
                },
                new Schedule
                {
                    ScheduleId = 3,
                    UserId = 2,
                    Description = "Bar",
                    ScheduleDate = DateTime.Now.AddDays(1)
                }
            };
        }

        public List<User> Users { get; set; }

        public List<Schedule> Schedules { get; set; }

        public int NextUserId
        {
            get
            {
                return Users.Count + 1;
            }
        }

        public int NextScheduleId
        {
            get
            {
                return Schedules.Count + 1;
            }
        }
    }
}
