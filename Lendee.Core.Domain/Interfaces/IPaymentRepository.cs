using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        void SaveNewPayments(IEnumerable<Payment> payments, long contractId);
        Task Save();
    }
}
