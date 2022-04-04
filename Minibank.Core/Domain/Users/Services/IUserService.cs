using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(User user);
        Task DeleteUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
    }
}
