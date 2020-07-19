using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Security.Claims;
using Todo.Controllers;
using Todo.DomainModels;
using Todo.DomainModels.Exceptions;
using Todo.Services.Interfaces;
using Xunit;

namespace Todo.API.Tests
{
    public class ScheduleControllerTests
    {
        private Mock<IScheduleService> _scheduleService;
        private readonly GetScheduleModel _getScheduleModel = new GetScheduleModel
        {
            ScheduleId = 1,
            Description = "Foo",
            ScheduleDate = DateTime.Now
        };
        private readonly ClaimsPrincipal _mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "1"),
        }, "mock"));

        public ScheduleControllerTests()
        {
            _scheduleService = new Mock<IScheduleService>();
        }

        [Fact]
        public async void ScheduleController_Update_Should_Return200_WhenServiceDoesNotThrowNotFound()
        {
            _scheduleService.Setup(service => service.UpdateSchedule(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UpdateScheduleModel>()));

            var controller = new ScheduleController(_scheduleService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser }
            };

            var result = await controller.Update(1, new UpdateScheduleModel
            {
                Description = "Bar",
                ScheduleDate = DateTime.Now
            });
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void ScheduleController_Update_Should_Return404_WhenServiceThrowsNotFound()
        {
            _scheduleService.Setup(service => service.UpdateSchedule(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UpdateScheduleModel>())).Throws(new NotFoundException());

            var controller = new ScheduleController(_scheduleService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser }
            };

            var result = await controller.Update(1, new UpdateScheduleModel
            {
                Description = "Bar",
                ScheduleDate = DateTime.Now
            });
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async void ScheduleController_Delete_Should_Return200_WhenServiceDoesNotThrowNotFound()
        {
            _scheduleService.Setup(service => service.DeleteSchedule(It.IsAny<int>(), It.IsAny<int>()));

            var controller = new ScheduleController(_scheduleService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser }
            };

            var result = await controller.Delete(1);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void ScheduleController_Delete_Should_Return404_WhenServiceThrowsNotFound()
        {
            _scheduleService.Setup(service => service.DeleteSchedule(It.IsAny<int>(), It.IsAny<int>())).Throws(new NotFoundException());

            var controller = new ScheduleController(_scheduleService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser }
            };

            var result = await controller.Delete(1);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
