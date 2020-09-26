using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.PaymentDues
{
    public class PaymentDueService
    {
        private readonly IPaymentDueRepository repository;

        public PaymentDueService(IPaymentDueRepository repository)
        {
            this.repository = repository;
        }

        public async Task Create(IEnumerable<PaymentDue> dues)
        {
            foreach (var due in dues)
                repository.Add(due);

            await repository.Save();
        }

        public async Task<List<PaymentDue>> List(long contractId)
        {
            return await repository.List(contractId);
        }
    }
}
