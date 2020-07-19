using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DataModels;
using Todo.Repositories.Interfaces;
using Xunit;

namespace Todo.Services.Tests
{
    public class AuthServiceTests
    {
        private AuthService _authService;

        public AuthServiceTests()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository
                .Setup(m => m.QueryUsers(It.IsAny<Func<User, bool>>()))
                .Returns((Func<User, bool> whereClause) =>
                {
                    var users = new List<User> {
                        new User { UserId = 1, Token = "Foo" },
                    };

                    return Task.FromResult(users.Where(whereClause).ToList());
                });

            _authService = new AuthService(mockUserRepository.Object);
        }

        [Theory]
        [InlineData("Foo", 1)]
        [InlineData("Bar", null)]
        public async void GetUserByToken_Should_Return_Null_WhenUserDoesNotExist(string token, int? expectedUserId)
        {
            var userId = await _authService.GetUserIdByToken(token);
            userId.Should().Be(expectedUserId);
        }
    }
}
