using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Core.Domain.Repayments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class LoanBuilderController : Controller
    {
        private readonly IContractRepository contractRepository;
        private readonly IContractDraftRepository draftRepository;
        private readonly IRepaymentRepository repaymentRepository;

        public LoanBuilderController(
            IContractRepository contractRepository,
            IContractDraftRepository draftRepository,
            IRepaymentRepository repaymentRepository)
        {
            this.contractRepository = contractRepository;
            this.draftRepository = draftRepository;
            this.repaymentRepository = repaymentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Loan(long contractId)
        {
            var loan = await contractRepository.FindLoan(contractId);
            return View(new LoanViewModel()
            {
                ContractId = contractId,
                Amount = loan.Amount,
                ValidFrom = loan.ValidFrom == default ? DateTime.Now : loan.ValidFrom,
                ValidUntil = loan.ValidUntil,
                PaymentTermType = loan.PaymentTermType,
                Day = loan.PaymentTermData?.Day,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Loan(LoanViewModel model)
        {
            var loan = await contractRepository.FindLoan(model.ContractId);
            loan.ValidFrom = model.ValidFrom;
            loan.ValidUntil = model.ValidUntil;
            loan.PaymentTermType = model.PaymentTermType;
            loan.Amount = model.Amount;
            loan.PaymentTermData = new PaymentTerm() { Day = model.Day };
            return await IncreaseDraftStepAndRedirect(model.ContractId);
        }

        [HttpGet]
        public async Task<IActionResult> LoanRepayments(long contractId)
        {
            var loan = await contractRepository.FindLoan(contractId);
            var repayments = new LoanRepaymentFactory().Generate(loan)
                .Select(x => new RepaymentItemViewModel() { Due = x.DueDate, Amount = x.Amount })
                .ToList();
            return View(new RepaymentViewModel() { ContractId = contractId, Repayments = repayments });
        }

        [HttpPost]
        public async Task<IActionResult> LoanRepayments(long contractId, RepaymentViewModel model)
        {
            var loan = await contractRepository.FindLoan(contractId);
            model.Repayments
                .Select(x => new LoanRepayment() { Amount = x.Amount, DueDate = x.Due, ContractId = contractId })
                .ToList()
                .ForEach(x => repaymentRepository.Add(x));
            await repaymentRepository.Save();

            return await IncreaseDraftStepAndRedirect(contractId);
        }

        [HttpGet]
        public async Task<IActionResult> LoanWithInterest(long contractId)
        {
            var loan = await contractRepository.FindLoanWithInterest(contractId);
            return View(new LoanWithInterestViewModel()
            {
                ContractId = contractId,
                Principal = loan.Amount,
                ValidFrom = loan.ValidFrom == default ? DateTime.Now : loan.ValidFrom,
                ValidUntil = loan.ValidUntil,
                PaymentTermType = loan.PaymentTermType,
                Day = loan.PaymentTermData?.Day,
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoanWithInterest(long contractId, LoanWithInterestViewModel model)
        {
            var loan = await contractRepository.FindLoanWithInterest(contractId);
            loan.Amount = model.Principal;
            loan.ValidFrom = model.ValidFrom;
            loan.ValidUntil = model.ValidUntil;
            loan.InterestRate = model.InterestRate;
            loan.PaymentTermType = PaymentTermType.Monthly;
            loan.PaymentTermData = new PaymentTerm() { Day = model.DueDay };

            model.Repayments
                .OrderBy(x => x.Due)
                .ToList()
                .ForEach(r => repaymentRepository.Add(new LoanWithInterestRepayment()
                {
                    Amount = r.Amount,
                    DueDate = r.Due,
                    ContractId = model.ContractId,
                    Interest = r.Interest
                }));

            await contractRepository.Save();

            return await IncreaseDraftStepAndRedirect(model.ContractId);
        }

        private async Task<IActionResult> IncreaseDraftStepAndRedirect(long contractId)
        {
            var draft = await draftRepository.Find(contractId);
            draft.Step += 1;
            await draftRepository.Save();
            return RedirectToAction(nameof(ContractBuilderController.Step), nameof(ContractBuilderController).Replace("Controller", ""), new { contractId = contractId });
        }

        public class LoanViewModel
        {
            public long ContractId { get; set; }
            public decimal Amount { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public PaymentTermType PaymentTermType { get; set; }
            public int? Day { get; set; }
        }

        public class LoanWithInterestViewModel
        {
            public long ContractId { get; set; }
            public decimal Principal { get; set; }
            public decimal InterestRate { get; set; }
            public int DueDay { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public PaymentTermType PaymentTermType { get; set; }
            public List<FullRepaymentItemViewModel> Repayments { get; set; }
            public int? Day { get; set; }
        }

        public class RepaymentViewModel
        {
            public long ContractId { get; set; }
            public List<RepaymentItemViewModel> Repayments { get; set; }
        }

        public class RepaymentItemViewModel
        {
            public DateTime Due { get; set; }
            public decimal Amount { get; set; }

        }

        public class FullRepaymentItemViewModel
        {
            public decimal TotalDebt { get; set; }
            public decimal Amount { get; set; }
            public decimal Debt { get; set; }
            public decimal Interest { get; set; }
            public DateTime Due { get; set; }
        }
    }
}