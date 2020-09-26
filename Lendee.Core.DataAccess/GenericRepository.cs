using Lendee.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly LendeeContext context;
        protected readonly DbSet<T> set;

        public GenericRepository(LendeeContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }
        public T Add(T entity)
        {
            var saved = set.Add(entity);
            return saved.Entity;
        }

        public ValueTask<T> Find(params object[] keys)
        {
            return set.FindAsync(keys);
        }

        public async Task<T> Remove(params object[] keys)
        {
            var entity = await Find(keys);
            var removed = set.Remove(entity);
            return removed.Entity;
        }

        public Task Save()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
