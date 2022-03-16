using System;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        void PostUser(User user);
        void DeleteUserById(string id);
        void PutUser(User user);
    }
}
