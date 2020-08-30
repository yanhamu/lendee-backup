using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Web.Features.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractBuilderController : Controller
    {
        private readonly IEntityRepository entityRepository;
        private readonly IContractRepository contractRepository;
        private readonly LegalEntityFactory legalEntityFactory;
        private readonly IContractDraftRepository draftRepository;

        public ContractBuilderController(
            IEntityRepository entityRepository,
            IContractRepository contractRepository,
            LegalEntityFactory legalEntityFactory,
            IContractDraftRepository contractDraftRepository)
        {
            this.entityRepository = entityRepository;
            this.contractRepository = contractRepository;
            this.legalEntityFactory = legalEntityFactory;
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
            if (ModelState.IsValid == false)
                return View(contract);
            var toSave = new Core.Domain.Model.Contract()
            {
                Currency = contract.Currency.Value,
                Name = contract.Name,
                Note = contract.Note,
                Type = contract.Type.Value
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

            if (draft.Step == 1)
                return ContractInitialization(contractId, contract);
            if (draft.Step == 2)
                return RedirectToAction(nameof(SetLendee), new { contractId });
            if (draft.Step == 3)
                return RedirectToAction(nameof(SetLender), new { contractId });
            if (draft.Step == 4 && contract.Type != ContractType.LoanWithInterest)
                return await Repayments(contractId);

            return RedirectToAction("List", "Contracts");
        }

        private IActionResult ContractInitialization(long contractId, Core.Domain.Model.Contract contract)
        {
            return contract.Type switch
            {
                ContractType.Undefined => throw new ArgumentException(),
                ContractType.Rent => RedirectToAction(nameof(RentBuilderController.Rent), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.CombinedRent => RedirectToAction(nameof(RentBuilderController.CombinedRent), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.VariableRent => RedirectToAction(nameof(RentBuilderController.VariableRent), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.Loan => RedirectToAction(nameof(LoanBuilderController.Loan), nameof(LoanBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.LoanWithInterest => RedirectToAction(nameof(LoanBuilderController.LoanWithInterest), nameof(LoanBuilderController).Replace("Controller", ""), new { contractId }),
                _ => throw new ArgumentException(),
            };
            throw new NotImplementedException($"{contract.Type} is not implemented yet");
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

        public async Task<IActionResult> Repayments(long contractId)
        {
            var contract = await contractRepository.Find(contractId);
            return contract.Type switch
            {
                ContractType.Undefined => throw new ArgumentException(),
                ContractType.Loan => RedirectToAction(nameof(LoanBuilderController.LoanRepayments), nameof(LoanBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.CombinedRent => RedirectToAction(nameof(RentBuilderController.CombinedRentRepayments), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.VariableRent => RedirectToAction(nameof(RentBuilderController.VariableRentRepayments), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                ContractType.Rent => RedirectToAction(nameof(RentBuilderController.RentRepayments), nameof(RentBuilderController).Replace("Controller", ""), new { contractId }),
                _ => throw new ArgumentException(),
            };
        }

        private async Task<IActionResult> IncreaseDraftStepAndRedirect(long contractId)
        {
            var draft = await draftRepository.Find(contractId);
            draft.Step += 1;
            await draftRepository.Save();
            return RedirectToAction(nameof(ContractBuilderController.Step), new { contractId });
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
        [Required]
        public string Name { get; set; }
        [Required]
        public ContractType? Type { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        public long? LenderId { get; set; }
        public long? LendeeId { get; set; }
        public string Note { get; set; }
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