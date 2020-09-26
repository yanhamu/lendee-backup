using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class PaymentDueRepository : GenericRepository<PaymentDue>, IPaymentDueRepository
    {
        public PaymentDueRepository(LendeeContext context) : base(context) { }

        public Task<List<PaymentDue>> List(long contractId)
        {
            return set.Where(x => x.ContractId == contractId).ToListAsync();
        }
    }
}
