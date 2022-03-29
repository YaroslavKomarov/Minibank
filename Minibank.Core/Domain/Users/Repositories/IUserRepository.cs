using System;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(string id);
        string CreateUser(User user);
        bool DeleteUserById(string id);
        bool UpdateUser(User user);
    }
}
