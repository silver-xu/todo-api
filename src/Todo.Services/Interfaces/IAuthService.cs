using System.Threading.Tasks;

namespace Todo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int?> GetUserIdByToken(string token);
    }
}
