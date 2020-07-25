using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public Payment Add(Payment payment)
        {
            var entity = context
                .Set<Payment>()
                .Add(payment);
            return entity.Entity;
        }

        public async Task<IEnumerable<Payment>> GetLast(int take, int skip)
        {
            return await context
                .Set<Payment>()
                .OrderByDescending(x => x.PaidAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}