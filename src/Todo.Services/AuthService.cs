using System.Linq;
using System.Threading.Tasks;
using Todo.Repositories.Interfaces;
using Todo.Services.Interfaces;

namespace Todo.Services
{
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int?> GetUserIdByToken(string token)
        {
            var user = (await _userRepository.QueryUsers(u => u.Token == token)).FirstOrDefault();

            return user?.UserId;
        }
    }
}
