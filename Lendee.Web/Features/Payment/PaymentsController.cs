using Lendee.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Payment
{
    public class PaymentsController : Controller
    {
        private readonly IRepaymentRepository repository;

        public PaymentsController(IRepaymentRepository repository)
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
            var payment = new Core.Domain.Model.Repayment()
            {
                Amount = model.Amount,
                ContractId = model.ContractId,
                PaidAt = model.PaidAt
            };

            repository.Add(payment);
            await repository.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var payment = await repository.Find(id);
            return View(new PaymentViewModel()
            {
                Id = payment.Id,
                Amount = payment.Amount,
                ContractId = payment.ContractId,
                PaidAt = payment.PaidAt
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PaymentViewModel model)
        {
            var payment = await repository.Find(model.Id);

            payment.Amount = model.Amount;
            payment.ContractId = model.ContractId;
            payment.PaidAt = model.PaidAt;

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