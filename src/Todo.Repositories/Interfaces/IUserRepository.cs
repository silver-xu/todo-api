using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataModels;

namespace Todo.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> QueryUsers(Func<User, bool> predicate);
    }
}
