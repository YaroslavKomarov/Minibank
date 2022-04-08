using Minibank.Core;
using System.Threading.Tasks;

namespace Minibank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly MinibankContext context;

        public EfUnitOfWork(MinibankContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
