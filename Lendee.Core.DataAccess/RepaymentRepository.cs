using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class RepaymentRepository : IRepaymentRepository
    {
        private readonly LendeeContext context;

        public RepaymentRepository(LendeeContext context)
        {
            this.context = context;
        }

        public Repayment Add<T>(T repayment) where T : Repayment
        {
            var entity = context
                .Set<T>()
                .Add(repayment);
            return entity.Entity;
        }

        public ValueTask<Repayment> Find(long id)
        {
            return context.Set<Repayment>()
                .FindAsync(id);
        }

        public async Task<IEnumerable<Repayment>> GetLast(int take, int skip)
        {
            return await context
                .Set<Repayment>()
                .OrderByDescending(x => x.DueDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Repayment>> List(long contractId)
        {
            return await context.Set<Repayment>().Where(x => x.ContractId == contractId).OrderByDescending(x => x.DueDate).ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}