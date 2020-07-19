using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DataModels;
using Todo.DataModels.Interfaces;
using Todo.Repositories.Interfaces;

namespace Todo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ITodoContext _todoContext;

        public UserRepository(ITodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task<List<User>> QueryUsers(Func<User, bool> predicate)
        {
            return await Task.Run(() =>
            {
                return _todoContext.Users.Where(predicate).ToList();
            });
        }
    }
}
