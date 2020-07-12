using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Lendee.Web.Features.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractBuilderController : Controller
    {
        private readonly IEntityRepository entityRepository;
        private readonly IContractRepository contractRepository;

        public ContractBuilderController(
            IEntityRepository entityRepository,
            IContractRepository contractRepository)
        {
            this.entityRepository = entityRepository;
            this.contractRepository = contractRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DraftContract());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DraftContract contract)
        {
            var toSave = new Lendee.Core.Domain.Model.Contract()
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
        public IActionResult SetLendee(long contractId)
        {
            ViewData["contractId"] = contractId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLendee(long contractId, EntityViewModel model)
        {
            var entity = new LegalEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber,
                Note = model.Note,
                CompanyName = model.CompanyName,
                IdentifyingNumber = model.IdentifyingNumber,
                TaxIdentifyingNumber = model.TaxIdentifyingNumber
            };

            var saved = entityRepository.Add(entity);
            await entityRepository.Save();

            var contract = await contractRepository.Find(contractId);
            contract.LendeeId = saved.Id;
            await contractRepository.Save();

            return RedirectToAction("SetLender", new { contractId = contractId });
        }

        [HttpGet]
        public IActionResult SetLender(long contractId)
        {
            ViewData["contractId"] = contractId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLender(long contractId, EntityViewModel model)
        {
            var entity = new LegalEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber,
                Note = model.Note,
                CompanyName = model.CompanyName,
                IdentifyingNumber = model.IdentifyingNumber,
                TaxIdentifyingNumber = model.TaxIdentifyingNumber
            };

            var saved = entityRepository.Add(entity);
            await entityRepository.Save();

            var contract = await contractRepository.Find(contractId);
            contract.LenderId = saved.Id;
            await contractRepository.Save();

            return RedirectToAction("SetLender", new { contractId = contractId });
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
}