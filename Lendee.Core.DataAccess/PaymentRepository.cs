using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly LendeeContext context;

        public PaymentRepository(LendeeContext context)
        {
            this.context = context;
        }
        public Task Save()
        {
            return context.SaveChangesAsync();
        }

        public void SaveNewPayments(IEnumerable<Payment> payments, long contractId)
        {
            foreach (var payment in payments)
            {
                context.Set<Payment>().Add(payment);
            }
        }
    }
}
