using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Core.Domain.Payments;
using Lendee.Web.Features.Common;
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
        private readonly IContractDraftRepository draftRepository;

        public ContractBuilderController(
            IEntityRepository entityRepository,
            IContractRepository contractRepository,
            IPaymentRepository paymentsRepository,
            LegalEntityFactory legalEntityFactory,
            PaymentFactory paymentFactory,
            IContractDraftRepository contractDraftRepository)
        {
            this.entityRepository = entityRepository;
            this.contractRepository = contractRepository;
            this.legalEntityFactory = legalEntityFactory;
            this.paymentFactory = paymentFactory;
            this.paymentsRepository = paymentsRepository;
            this.draftRepository = contractDraftRepository;
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
            draftRepository.Add(new ContractDraft() { ContractId = saved.Id, Step = 1 });
            await draftRepository.Save();
            return RedirectToAction("Step", new { contractId = saved.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Step(long contractId)
        {
            var draft = await draftRepository.Find(contractId);
            var contract = await contractRepository.Find(contractId);

            if (draft.Step == 1 && contract.Type == ContractType.Rent)
                return RedirectToAction(nameof(Rent), new { contractId });

            if (draft.Step == 2)
                return RedirectToAction(nameof(SetLendee), new { contractId });
            if (draft.Step == 3)
                return RedirectToAction(nameof(SetLender), new { contractId });

            if (draft.Step == 4)
                return RedirectToAction(nameof(Payments), new { contractId });

            return RedirectToAction("List", "Contracts");
        }

        [HttpGet]
        public async Task<IActionResult> SetLendee(long contractId)
        {
            var contract = await contractRepository.Find(contractId);
            var entities = await entityRepository.List();
            return View(new ContractEntityViewModel() { Entities = entities, Selected = contract.LendeeId, ContractId = contractId });
        }

        [HttpPost]
        public async Task<IActionResult> SetLendee(long contractId, ContractEntityViewModel model)
        {
            var contract = await contractRepository.Find(contractId);
            contract.LendeeId = model.Selected;
            await contractRepository.Save();

            return await IncreaseDraftStepAndRedirect(contractId);
        }

        [HttpGet]
        public async Task<IActionResult> SetLender(long contractId)
        {
            var contract = await contractRepository.Find(contractId);
            var entities = await entityRepository.List();
            return View(new ContractEntityViewModel() { Entities = entities, Selected = contract.LenderId, ContractId = contractId });
        }

        [HttpPost]
        public async Task<IActionResult> SetLender(long contractId, ContractEntityViewModel model)
        {
            var contract = await contractRepository.Find(contractId);
            contract.LenderId = model.Selected;
            await contractRepository.Save();

            return await IncreaseDraftStepAndRedirect(contractId);
        }

        [HttpGet]
        public async Task<IActionResult> Rent(long contractId)
        {
            var rent = await contractRepository.FindRent(contractId);
            return View(new RentViewModel()
            {
                ContractId = rent.Id,
                PaymentTermType = rent.PaymentTermType,
                RentType = rent.RentType,
                PaymentAmount = rent.PaymentAmount,
                ValidUntil = rent.ValidUntil,
                ValidFrom = DateTime.Now,
                Day = rent.PaymentTermData?.Day,
                Month = rent.PaymentTermData?.Month
            });
        }

        [HttpPost]
        public async Task<IActionResult> Rent(RentViewModel model)
        {
            var rent = await contractRepository.FindRent(model.ContractId);
            rent.PaymentTermType = model.PaymentTermType;
            rent.RentType = model.RentType;
            rent.PaymentAmount = model.PaymentAmount;
            rent.ValidFrom = model.ValidFrom;
            rent.ValidUntil = model.ValidUntil;
            rent.PaymentTermData = new PaymentTerm()
            {
                Day = model.Day,
                Month = model.Month
            };

            await contractRepository.Save();
            return await IncreaseDraftStepAndRedirect(model.ContractId);
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
            return await IncreaseDraftStepAndRedirect(model.ContractId);
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

        private async Task<IActionResult> IncreaseDraftStepAndRedirect(long contractId)
        {
            var draft = await draftRepository.Find(contractId);
            draft.Step += 1;
            await draftRepository.Save();
            return RedirectToAction("Step", new { contractId = contractId });
        }

        public class PaymentsViewModel
        {
            public List<Core.Domain.Model.Payment> Payments { get; set; }
            public long ContractId { get; set; }
        }

        public class ContractEntityViewModel
        {
            public long ContractId { get; set; }
            public IEnumerable<LegalEntity> Entities { get; set; }
            public long? Selected { get; set; }
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
        public decimal? Fee { get; set; }
        public decimal? NormalizedFee { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public PaymentTermType PaymentTermType { get; set; }
        public RentType RentType { get; set; }
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