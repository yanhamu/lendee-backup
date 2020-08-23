using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendee.Web.Features.Contract
{
    public class RepaymentCalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new CreditViewModel());
        }

        [HttpPost]
        public IActionResult Index(CreditViewModel model)
        {
            var interestRate = model.InterestRate / 100;
            var p = Math.Pow((1 + (double)interestRate), model.PaymentsCount);
            var payment = (double)model.Amount * ((double)interestRate * p / (p - 1));
            var ceiledPayment = (decimal)Math.Ceiling(payment);

            var payments = GeneratePayments(ceiledPayment, interestRate, model.Amount, model.PaymentsCount).ToList();
            return View(new CreditViewModel() { Amount = model.Amount, InterestRate = model.InterestRate, PaymentsCount = model.PaymentsCount, Payments = payments });
        }

        private IEnumerable<PaymentViewModel> GeneratePayments(decimal payment, decimal interestRate, decimal total, int count)
        {
            var totalDebt = total;
            for (int i = 0; i < count - 1; i++)
            {
                var interest = Math.Round(totalDebt * interestRate);
                var debt = payment - interest;
                yield return new PaymentViewModel() { Debt = debt, Interest = interest, TotalDebt = totalDebt };
                totalDebt -= debt;
            }
            {
                var interest = Math.Round(totalDebt * interestRate);
                yield return new PaymentViewModel() { Debt = totalDebt, Interest = interest, TotalDebt = totalDebt };
            }
        }

        public class CreditViewModel
        {
            public decimal Amount { get; set; }
            public decimal InterestRate { get; set; }
            public int PaymentsCount { get; set; }
            public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        }

        public class PaymentViewModel
        {
            public decimal Payment { get => Debt + Interest; }
            public decimal Debt { get; set; }
            public decimal Interest { get; set; }
            public decimal TotalDebt { get; set; }
        }
    }
}
