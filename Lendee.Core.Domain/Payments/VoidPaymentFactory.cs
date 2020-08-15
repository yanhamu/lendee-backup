using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Lendee.Core.Domain.Payments
{
    public class VoidPaymentFactory
    {
        public IEnumerable<Payment> BuildPayments(long contractId, PaymentSettings paymentSetting)
        {
            return Enumerable.Empty<Payment>();
        }
    }
}
