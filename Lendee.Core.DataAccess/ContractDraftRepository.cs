using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class ContractDraftRepository : IContractDraftRepository
    {
        private readonly LendeeContext context;

        public ContractDraftRepository(LendeeContext context)
        {
            this.context = context;
        }

        public ContractDraft Add(ContractDraft contractDraft)
        {
            var entity = this.context.Set<ContractDraft>().Add(contractDraft);
            return entity.Entity;
        }

        public ValueTask<ContractDraft> Find(long contractId)
        {
            return context.Set<ContractDraft>().FindAsync(contractId);
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}
