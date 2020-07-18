using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Web.Features.Common;
using Lendee.Web.Features.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractBuilderController : Controller
    {
        private readonly IEntityRepository entityRepository;
        private readonly IContractRepository contractRepository;
        private readonly LegalEntityFactory legalEntityFactory;

        public ContractBuilderController(
            IEntityRepository entityRepository,
            IContractRepository contractRepository,
            LegalEntityFactory legalEntityFactory)
        {
            this.entityRepository = entityRepository;
            this.contractRepository = contractRepository;
            this.legalEntityFactory = legalEntityFactory;
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

            return RedirectToAction("SetLender", new { contractId = contractId });
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
                    break;
                case ContractType.Loan:
                    break;
                case ContractType.Rent:
                    return RedirectToAction("Rent", new { contractId = contractId });
                default:
                    throw new ArgumentException();
            }

            return RedirectToAction("SetLender", new { contractId = contractId });
        }

        [HttpGet]
        public async Task<IActionResult> Rent(long? contractId)
        {
            if (contractId.HasValue == false)
                return View(new RentViewModel());

            var contract = await contractRepository.FindRent(contractId.Value);
            return View(new RentViewModel()
            {
                ContractId = contract.Id,
                PaymentTermType = contract.PaymentTermType,
                PaymentAmount = contract.PaymentAmount,
                ValidUntil = contract.ValidUntil,
                ValidFrom = DateTime.Now
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

            await contractRepository.Save();
            return RedirectToAction("List", "Contracts");
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
        public PaymentTerm PaymentTermType { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
    }
}