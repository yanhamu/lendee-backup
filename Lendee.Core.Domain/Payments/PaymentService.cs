using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Payments
{
    public class PaymentService
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            var saved = paymentRepository.Add(payment);
            await paymentRepository.Save();
            return saved;
        }

        public async Task<IEnumerable<Payment>> List(long contractId)
        {
            return await paymentRepository.List(contractId);
        }
    }
}