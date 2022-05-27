using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task DeleteUserByIdAsync(string id, CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    }
}
