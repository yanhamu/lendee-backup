using Lendee.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Payment
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentRepository repository;

        public PaymentsController(IPaymentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var payments = await repository.GetLast(100, 0);
            return View(payments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PaymentViewModel() { PaidAt = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            var payment = new Core.Domain.Model.Payment()
            {
                Amount = model.Amount,
                ContractId = model.ContractId,
                PaidAt = model.PaidAt
            };

            repository.Add(payment);
            await repository.Save();
            return RedirectToAction(nameof(List));
        }
    }

    public class PaymentViewModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public long ContractId { get; set; }
    }
}