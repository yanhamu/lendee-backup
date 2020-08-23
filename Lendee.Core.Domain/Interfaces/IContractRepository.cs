using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IContractRepository
    {
        Contract Add(Contract toSave);
        Task Save();
        Task<Contract> Find(long contractId);
        Task<IEnumerable<Contract>> GetAll();
        ValueTask<Rent> FindRent(long contractId);
        ValueTask<VariableRent> FindVariableRent(long contractId);
        ValueTask<CombinedRent> FindCombinedRent(long contractId);
        ValueTask<Loan> FindLoan(long contractId);
    }
}
