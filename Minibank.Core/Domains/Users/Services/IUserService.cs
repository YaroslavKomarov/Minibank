using System;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        void CreateUser(User user);
        void DeleteUserById(string id);
        void UpdateUser(User user);
    }
}
