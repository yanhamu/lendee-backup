using Lendee.Core.Domain.Model;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IContractDraftRepository
    {
        ValueTask<ContractDraft> Find(long contractId);
        Task Save();
        ContractDraft Add(ContractDraft contractDraft);
    }
}
