using Minibank.Core.Domains.Users.Repositories;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Users;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MinibankContext context;

        public UserRepository(MinibankContext context)
        {
            this.context = context;
        }

        public async Task<bool> DeleteUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            var userModel = await context.Users.FirstOrDefaultAsync(it => it.Id == id);
            
            if (userModel != null)
            {
                context.Users.Remove(userModel);

                return true;
            }

            return false;
        }

        public async Task<User> GetUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            var userModel = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id);

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

        public async Task<string> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();

            await context.Users.AddAsync(new UserDbModel
            {
                Id = id,
                Login = user.Login,
                Email = user.Email
            });

            return id;
        }

        public async Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var oldUserModel = await context.Users.FirstOrDefaultAsync(it => it.Id == user.Id);

            if (oldUserModel != null)
            {
                oldUserModel.Login = user.Login;
                oldUserModel.Email = user.Email;

                return true;
            }

            return false;
        }

        public async Task<bool> CheckIsLoginUniqueAsync(string login, CancellationToken cancellationToken)
        {
            if (await context.Users.FirstOrDefaultAsync(it => it.Login == login) != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            if (await context.Users.FirstOrDefaultAsync(it => it.Email == email) != null)
            {
                return false;
            }

            return true;
        }
    }
}
