using Lendee.Core.Domain.PaymentDues;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Web.Features.PaymentDue
{
    [Route("api")]
    public class PaymentDuesController : ControllerBase
    {
        private readonly PaymentDueService service;

        public PaymentDuesController(PaymentDueService service)
        {
            this.service = service;
        }

        [Route("contracts/{contractId:long}/payment-dues")]
        [HttpPost]
        public async Task<IActionResult> Create(long contractId, [FromBody] List<CreatePaymentDueRequest> dues)
        {
            await service.Create(dues.Select(x => x.Create(contractId)));
            return Ok();
        }

        [Route("contracts/{contractId:long}/payment-dues")]
        [HttpGet]
        public async Task<IEnumerable<PaymentDueModel>> List(long contractId)
        {
            var payments = await service.List(contractId);
            return payments.Select(x => new PaymentDueModel(x));
        }

        public class CreatePaymentDueRequest
        {
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public Core.Domain.Model.PaymentDue Create(long contractId)
            {
                return new Core.Domain.Model.PaymentDue()
                {
                    Amount = this.Amount,
                    ContractId = contractId,
                    Due = this.Date
                };
            }
        }

        public class PaymentDueModel
        {
            public long Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public PaymentDueModel(Core.Domain.Model.PaymentDue paymentDue)
            {
                this.Id = paymentDue.Id;
                this.Amount = paymentDue.Amount;
                this.Date = paymentDue.Due;
            }
        }
    }
}
