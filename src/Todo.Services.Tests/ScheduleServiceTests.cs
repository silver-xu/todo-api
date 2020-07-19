using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DataModels;
using Todo.DomainModels;
using Todo.DomainModels.Exceptions;
using Todo.Mappers;
using Todo.Repositories.Interfaces;
using Xunit;

namespace Todo.Services.Tests
{
    public class ScheduleServiceTests
    {
        private Mock<IScheduleRepository> _mockScheduleRepository;
        private static Schedule _mockSchedule = new Schedule { ScheduleId = 1, UserId = 1, Description = "FooBar", ScheduleDate = DateTime.Now };
        private static List<Schedule> _mockSchedules = new List<Schedule> { _mockSchedule };

        public ScheduleServiceTests()
        {
            _mockScheduleRepository = new Mock<IScheduleRepository>();
        }

        public static IEnumerable<object[]> GetSchedulesByUserIdInlineData
        {
            get
            {
                return new[]
                {
                    new object[] { 1, _mockSchedules},
                    new object[] { 2, new List<Schedule>{ } },
                };
            }
        }

        [Theory, MemberData(nameof(GetSchedulesByUserIdInlineData))]
        public async void GetSchedulesByUserId_Should_Return_Null_WhenUserDoesNotExist(int userId, List<Schedule> expectedSchedules)
        {
            _mockScheduleRepository
                .Setup(m => m.QuerySchedules(It.IsAny<Func<Schedule, bool>>()))
                .Returns((Func<Schedule, bool> whereClause) =>
                {
                    return Task.FromResult(new List<Schedule> { _mockSchedule }.Where(whereClause).ToList());
                });

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);


            var schedules = await scheduleService.GetSchedulesByUserId(userId);
            schedules.Should().BeEquivalentTo(expectedSchedules.Select(schedule => ScheduleMapper.MapToGetScheduleModel(schedule)));
        }


        [Fact]
        public void GetSchedule_Should_Throw_NotFoundException_WhenScheduleNotExist()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult<Schedule>(null));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.GetSchedule(1, 1);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void GetSchedule_Should_Throw_NotFoundException_WhenScheduleNotCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.GetSchedule(2, 1);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async void GetSchedule_Should_CallScheduleRepository_GetSchedule_WhenScheduleExistsAndCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            await scheduleService.GetSchedule(1, 1);

            _mockScheduleRepository.Verify(s => s.GetSchedule(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateSchedule_Should_Throw_NotFoundException_WhenScheduleNotExist()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult<Schedule>(null));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.UpdateSchedule(1, 1, new UpdateScheduleModel
            {
                Description = "Foo",
                ScheduleDate = DateTime.Now
            });

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void UpdateSchedule_Should_Throw_NotFoundException_WhenScheduleNotCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.UpdateSchedule(2, 1, new UpdateScheduleModel
            {
                Description = "Foo",
                ScheduleDate = DateTime.Now
            });

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async void UpdateSchedule_Should_CallScheduleRepository_UpdateSchedule_WhenScheduleExistsAndCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));
            _mockScheduleRepository.Setup(m => m.UpdateSchedule(It.IsAny<Schedule>()));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            await scheduleService.UpdateSchedule(1, 1, new UpdateScheduleModel
            {
                Description = "Foo",
                ScheduleDate = DateTime.Now
            });

            _mockScheduleRepository.Verify(s => s.UpdateSchedule(It.IsAny<Schedule>()), Times.Once);
        }


        [Fact]
        public void DeleteSchedule_Should_Throw_NotFoundException_WhenScheduleNotExist()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult<Schedule>(null));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.DeleteSchedule(1, 1);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void DeleteSchedule_Should_Throw_NotFoundException_WhenScheduleNotCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            Func<Task> act = async () => await scheduleService.DeleteSchedule(2, 1);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async void DeleteSchedule_Should_CallScheduleRepository_DeleteSchedule_WhenScheduleExistsAndCreatedByUser()
        {
            _mockScheduleRepository.Setup(m => m.GetSchedule(It.IsAny<int>())).Returns(Task.FromResult(_mockSchedule));
            _mockScheduleRepository.Setup(m => m.DeleteSchedule(It.IsAny<int>()));

            var scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            await scheduleService.DeleteSchedule(1, 1);

            _mockScheduleRepository.Verify(s => s.DeleteSchedule(It.IsAny<int>()), Times.Once);
        }
    }
}
