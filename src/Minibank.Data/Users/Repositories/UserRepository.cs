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
            var userModel = await context.Users
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);
            
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
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

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
            var entry = await context.Users.AddAsync(new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            }, cancellationToken);

            return entry.Entity.Id;
        }

        public async Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var oldUserModel = await context.Users
                .FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken);

            if (oldUserModel != null)
            {
                context.Entry(oldUserModel).CurrentValues.SetValues(user);

                return true;
            }

            return false;
        }

        public async Task<bool> CheckIsLoginUniqueAsync(string login, CancellationToken cancellationToken)
        {
            return !await context.Users
                .AsNoTracking()
                .AnyAsync(it => it.Login == login, cancellationToken);
        }

        public async Task<bool> CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            return !await context.Users
                .AsNoTracking()
                .AnyAsync(it => it.Email == email, cancellationToken);
        }
    }
}
