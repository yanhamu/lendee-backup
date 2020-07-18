using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class ContractRepository : IContractRepository
    {
        private readonly LendeeContext context;

        public ContractRepository(LendeeContext context)
        {
            this.context = context;
        }

        public Contract Add(Contract toSave)
        {
            var saved = context.Set<Contract>().Add(toSave);
            return saved.Entity;
        }

        public async Task<Contract> Find(long contractId)
        {
            return await context.Set<Contract>().FindAsync(contractId);
        }

        public ValueTask<Rent> FindRent(long contractId)
        {
            return context.Set<Rent>().FindAsync(contractId);
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            return await context.Set<Contract>().ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}
