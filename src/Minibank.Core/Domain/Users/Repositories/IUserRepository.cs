using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id, CancellationToken cancellationToken);
        Task<string> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> DeleteUserByIdAsync(string id, CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> CheckIsLoginUniqueAsync(string login, CancellationToken cancellationToken);
        Task<bool> CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    }
}
