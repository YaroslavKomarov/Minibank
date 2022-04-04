using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<string> CreateUserAsync(User user);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> CheckIsUserLoginUniqueAsync(string login);
        Task<bool> CheckIsUserEmailUniqueAsync(string email);
    }
}
