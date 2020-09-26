using Lendee.Core.Domain.Payments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Payment
{
    [Route("api")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService service;

        public PaymentsController(PaymentService service)
        {
            this.service = service;
        }

        [Route("contracts/{contractId:long}/payments")]
        [HttpPost]
        public async Task<PaymentModel> Create(long contractId, [FromBody]CreatePaymentRequest payment)
        {
            var toSave = new Core.Domain.Model.Payment()
            {
                Amount = payment.Amount,
                ContractId = contractId,
                ReceivedAt = payment.ReceivedAt
            };

            var saved = await service.AddPayment(toSave);

            return new PaymentModel(saved);
        }

        [Route("contracts/{contractId:long}/payments")]
        [HttpGet]
        public async Task<IEnumerable<PaymentModel>> List(long contractId)
        {
            var payments = await service.List(contractId);
            return payments.Select(x => new PaymentModel(x));
        }

        public class CreatePaymentRequest
        {
            public decimal Amount { get; set; }
            public DateTime ReceivedAt { get; set; }
        }

        public class PaymentModel
        {
            public long Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime ReceivedAt { get; set; }
            public PaymentModel(Core.Domain.Model.Payment payment)
            {
                this.Amount = payment.Amount;
                this.Id = payment.Id;
                this.ReceivedAt = payment.ReceivedAt;
            }
        }
    }
}