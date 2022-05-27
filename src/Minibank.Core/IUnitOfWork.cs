using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
