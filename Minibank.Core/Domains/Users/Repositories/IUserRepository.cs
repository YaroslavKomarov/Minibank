using System;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(string id);
        void PostUser(User user);
        bool DeleteUserById(string id);
        bool PutUser(User user);
    }
}
