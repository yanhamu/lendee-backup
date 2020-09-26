using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.PaymentChecker
{
    public class PaymentCheckerService
    {
        public Task<IEnumerable<PaymentCheck>> Check(long contractId)
        {
            throw new NotImplementedException();
        }
    }

    public class PaymentCheck
    {

    }
}
