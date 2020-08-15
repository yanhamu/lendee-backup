using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IRepaymentRepository
    {
        Task<IEnumerable<Model.Repayment>> GetLast(int take, int skip);
        Model.Repayment Add(Model.Repayment payment);
        Task Save();
        ValueTask<Model.Repayment> Find(long id);
        Task<IEnumerable<Model.Repayment>> List(long contractId);
    }
}
