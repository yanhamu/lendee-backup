using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class EntityRepository : IEntityRepository
    {
        private readonly LendeeContext context;

        public EntityRepository(LendeeContext context)
        {
            this.context = context;
        }

        public LegalEntity Add(LegalEntity entity)
        {
            var saved = context.Set<LegalEntity>().Add(entity);
            context.SaveChanges();
            return saved.Entity;
        }

        public async Task<LegalEntity> Find(long id)
        {
            var result = await context.Set<LegalEntity>().FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<LegalEntity>> List()
        {
            var result = await context.Set<LegalEntity>().ToListAsync();
            return result;
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}