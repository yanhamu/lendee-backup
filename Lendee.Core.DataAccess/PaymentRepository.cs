using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Core.DataAccess
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(LendeeContext context) : base(context) { }

        public Task<List<Payment>> List(long contractId)
        {
            return set.Where(p => p.ContractId == contractId).OrderByDescending(x=>x.ReceivedAt).ToListAsync();
        }
    }
}
