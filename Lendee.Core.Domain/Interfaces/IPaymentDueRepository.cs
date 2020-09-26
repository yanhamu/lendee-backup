using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IPaymentDueRepository : IRepository<PaymentDue>
    {
        Task<List<PaymentDue>> List(long contractId);
    }
}
