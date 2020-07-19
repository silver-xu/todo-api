using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Todo.API.Middlewares;
using Todo.Services.Interfaces;
using Xunit;

namespace Todo.API.Tests.Middlewares
{
    public class AuthMiddlewareTests
    {
        private Mock<IAuthService> _mockAuthService;

        public AuthMiddlewareTests()
        {
            _mockAuthService = new Mock<IAuthService>();
        }

        [Fact]
        public async void ShouldInvalidate_WhenApiKeyIsNull()
        {
            RequestDelegate next = (HttpContext context) => Task.CompletedTask;
            var context = new DefaultHttpContext();

            var authMiddleware = new AuthMiddleware(next);

            await authMiddleware.InvokeAsync(context, _mockAuthService.Object);
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void ShouldInvalidate_WhenApiKeyIsInvalid()
        {
            _mockAuthService.Setup(service => service.GetUserIdByToken(It.IsAny<string>())).Returns(Task.FromResult<int?>(null));
            RequestDelegate next = (HttpContext context) => Task.CompletedTask;
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("X-API-Key", "foobar");

            var authMiddleware = new AuthMiddleware(next);

            await authMiddleware.InvokeAsync(context, _mockAuthService.Object);
            context.Response.StatusCode.Should().Be(401);
        }

        [Fact]
        public async void ShouldAssignIdentity_WhenApiKeyIsValid()
        {
            _mockAuthService.Setup(service => service.GetUserIdByToken(It.IsAny<string>())).Returns(Task.FromResult((int?)1));
            RequestDelegate next = (HttpContext context) => Task.CompletedTask;
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("X-API-Key", "foobar");

            var authMiddleware = new AuthMiddleware(next);

            await authMiddleware.InvokeAsync(context, _mockAuthService.Object);
            context.User.Identity.Name.Should().Be("1");
        }
    }
}
