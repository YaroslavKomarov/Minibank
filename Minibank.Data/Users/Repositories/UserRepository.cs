using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Data.Users.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private static List<UserDbModel> userStorage = new List<UserDbModel>();

        public bool DeleteUserById(string id)
        {
            var userModel = userStorage.FirstOrDefault(it => it.Id == id);
            
            if (userModel != null)
            {
                return userStorage.Remove(userModel);
            }

            return false;
        }

        public User GetUserById(string id)
        {
            var userModel = userStorage.FirstOrDefault(it => it.Id == id);

            if (userModel == null)
            {
                return null;
            }

            return new User
            {
                Id = userModel.Id,
                Login = userModel.Login,
                Email = userModel.Email,
            };
        }

        public string CreateUser(User user)
        {
            var id = Guid.NewGuid().ToString();

            userStorage.Add(new UserDbModel
            {
                Id = id,
                Login = user.Login,
                Email = user.Email
            });

            return id;
        }

        public bool UpdateUser(User user)
        {
            var oldUserModel = userStorage.FirstOrDefault(it => it.Id == user.Id);

            if (oldUserModel != null)
            {
                userStorage[userStorage.IndexOf(oldUserModel)] = new UserDbModel
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email
                };
                return true;
            }

            return false;
        }
    }
}
