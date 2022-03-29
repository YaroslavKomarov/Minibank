using System;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        string CreateUser(User user);
        void DeleteUserById(string id);
        void UpdateUser(User user);
    }
}
