using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Core.Domain.Payments;
using Lendee.Web.Features.Common;
using Lendee.Web.Features.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractBuilderController : Controller
    {
        private readonly IEntityRepository entityRepository;
        private readonly IContractRepository contractRepository;
        private readonly LegalEntityFactory legalEntityFactory;
        private readonly PaymentFactory paymentFactory;
        private readonly IPaymentRepository paymentsRepository;

        public ContractBuilderController(
            IEntityRepository entityRepository,
            IContractRepository contractRepository,
            IPaymentRepository paymentsRepository,
            LegalEntityFactory legalEntityFactory,
            PaymentFactory paymentFactory)
        {
            this.entityRepository = entityRepository;
            this.contractRepository = contractRepository;
            this.legalEntityFactory = legalEntityFactory;
            this.paymentFactory = paymentFactory;
            this.paymentsRepository = paymentsRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DraftContract());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DraftContract contract)
        {
            var toSave = new Core.Domain.Model.Contract()
            {
                Currency = contract.Currency,
                Name = contract.Name,
                Note = contract.Note,
                Type = contract.Type
            };

            var saved = contractRepository.Add(toSave);
            await contractRepository.Save();

            return RedirectToAction("SetLendee", new { contractId = saved.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SetLendee(long contractId)
        {
            ViewData["contractId"] = contractId;

            var contract = await contractRepository.Find(contractId);
            if (contract.LendeeId.HasValue)
            {
                var lendee = await entityRepository.Find(contract.LendeeId.Value);
                var model = legalEntityFactory.Create(lendee);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLendee(long contractId, EntityViewModel model)
        {
            var entity = legalEntityFactory.Create(model);
            var saved = entityRepository.Add(entity);
            await entityRepository.Save();

            var contract = await contractRepository.Find(contractId);
            contract.LendeeId = saved.Id;
            await contractRepository.Save();

            return RedirectToAction("SetLender", new { contractId });
        }

        [HttpGet]
        public async Task<IActionResult> SetLender(long contractId)
        {
            ViewData["contractId"] = contractId;

            var contract = await contractRepository.Find(contractId);
            if (contract.LenderId.HasValue)
            {
                var lender = await entityRepository.Find(contract.LenderId.Value);
                var model = legalEntityFactory.Create(lender);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLender(long contractId, EntityViewModel model)
        {
            ViewData["contractId"] = contractId;

            var entity = legalEntityFactory.Create(model);
            var saved = entityRepository.Add(entity);
            await entityRepository.Save();

            var contract = await contractRepository.Find(contractId);
            contract.LenderId = saved.Id;
            await contractRepository.Save();

            switch (contract.Type)
            {
                case ContractType.Credit:
                    return RedirectToAction("Credit", new { contractId });
                case ContractType.Loan:
                    break;
                case ContractType.Rent:
                    return RedirectToAction("Rent", new { contractId });
                default:
                    throw new ArgumentException();
            }

            return RedirectToAction("SetLender", new { contractId });
        }

        [HttpGet]
        public async Task<IActionResult> Rent(long contractId)
        {
            var contract = await contractRepository.FindRent(contractId);
            return View(new RentViewModel()
            {
                ContractId = contract.Id,
                PaymentTermType = contract.PaymentTermType,
                PaymentAmount = contract.PaymentAmount,
                ValidUntil = contract.ValidUntil,
                ValidFrom = DateTime.Now,
                Day = contract.PaymentTermData?.Day,
                Month = contract.PaymentTermData?.Month
            });
        }

        [HttpPost]
        public async Task<IActionResult> Rent(RentViewModel model)
        {
            var rent = await contractRepository.FindRent(model.ContractId);
            rent.PaymentTermType = model.PaymentTermType;
            rent.PaymentAmount = model.PaymentAmount;
            rent.ValidFrom = model.ValidFrom;
            rent.ValidUntil = model.ValidUntil;
            rent.PaymentTermData = new PaymentTerm()
            {
                Day = model.Day,
                Month = model.Month
            };

            await contractRepository.Save();
            return RedirectToAction("Payments", new { contracitId = model.ContractId });
        }

        [HttpGet]
        public async Task<IActionResult> Payments(long contractId)
        {
            var contract = await contractRepository.Find(contractId);
            var payments = paymentFactory.BuildPayments(contractId, contract.PaymentSettings);
            return View(new PaymentsViewModel() { ContractId = contractId, Payments = payments.ToList() });
        }

        [HttpPost]
        public async Task<IActionResult> Payments(PaymentsViewModel model)
        {
            var toSave = model.Payments.Select(x => new Lendee.Core.Domain.Model.Payment() { Amount = x.Amount, ContractId = model.ContractId, DueDate = x.DueDate });
            paymentsRepository.SaveNewPayments(toSave, model.ContractId);
            await paymentsRepository.Save();
            return RedirectToAction("List", "Contracts");
        }

        [HttpGet]
        public async Task<IActionResult> Credit(long contractId)
        {
            var credit = await contractRepository.FindCredit(contractId);
            return View(new CreditViewModel()
            {
                Id = credit.Id,
                InterestRate = credit.InterestRate,
                PaymentTermType = credit.PaymentTermType,
                PrincipalSum = credit.PrincipalSum,
                ValidFrom = credit.ValidFrom == default ? DateTime.Now : credit.ValidFrom,
                ValidUntil = credit.ValidUntil ?? DateTime.Now.AddYears(1),
                Day = credit.PaymentTermData?.Day,
                Month = credit.PaymentTermData?.Month
            });
        }

        [HttpPost]
        public async Task<IActionResult> Credit(CreditViewModel model)
        {
            var credit = await contractRepository.FindCredit(model.Id);
            credit.InterestRate = model.InterestRate;
            credit.PaymentTermType = model.PaymentTermType;
            credit.PaymentTermData = new PaymentTerm() { Day = model.Day, Month = model.Month };
            credit.PrincipalSum = model.PrincipalSum;
            credit.ValidFrom = model.ValidFrom;
            credit.ValidUntil = model.ValidUntil;

            await contractRepository.Save();

            return RedirectToAction("List", "Contracts");
        }

        public class PaymentsViewModel
        {
            public List<Core.Domain.Model.Payment> Payments { get; set; }
            public long ContractId { get; set; }
        }
    }

    public class DraftContract
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ContractType Type { get; set; }
        public Currency Currency { get; set; }
        public long? LenderId { get; set; }
        public long? LendeeId { get; set; }
        public string Note { get; set; }
    }

    public class RentViewModel
    {
        public long ContractId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public PaymentTermType PaymentTermType { get; set; }

        public int? Day { get; set; }
        public int? Month { get; set; }
    }

    public class CreditViewModel
    {
        public long Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PrincipalSum { get; set; }
        public PaymentTermType PaymentTermType { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
    }
}