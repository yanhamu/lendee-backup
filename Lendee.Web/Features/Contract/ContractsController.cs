using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractsController : Controller
    {
        private readonly IContractRepository contractRepository;
        private readonly IEntityRepository entityRepository;

        public ContractsController(IContractRepository repository, IEntityRepository entityRepository)
        {
            this.contractRepository = repository;
            this.entityRepository = entityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var contracts = await contractRepository.GetAll();

            return View(contracts);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(long id)
        {
            var contract = await contractRepository.Find(id);
            switch (contract.Type)
            {
                case ContractType.Draft:
                    break;
                case ContractType.Credit:
                    break;
                case ContractType.Loan:
                    break;
                case ContractType.Rent:
                    return RedirectToAction(nameof(DetailRent), new { id = id });
                default:
                    break;
            }

            throw new ArgumentException();
        }

        public async Task<IActionResult> DetailRent(long id)
        {
            var rent = await contractRepository.FindRent(id);
            var lender = await entityRepository.Find(rent.LenderId.Value);
            var lendee = await entityRepository.Find(rent.LendeeId.Value);
            var model = new RentDetailViewModel(rent, lender, lendee);
            return View(model);
        }
        public class RentDetailViewModel
        {
            public long ContractId { get; }
            public string Name { get; set; }
            public Currency Currency { get; set; }
            public string Note { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public EntityViewModel Lender { get; set; }
            public EntityViewModel Lendee { get; set; }
            public RentDetailViewModel(Rent rent, LegalEntity lender, LegalEntity lendee)
            {
                this.ContractId = rent.Id;
                this.Name = rent.Name;
                this.Currency = rent.Currency;
                this.Note = rent.Note;
                this.ValidFrom = rent.ValidFrom;
                this.ValidUntil = rent.ValidUntil;
                this.Lender = new EntityViewModel(lender);
                this.Lendee = new EntityViewModel(lendee);
            }
        }


        public class EntityViewModel
        {
            public long Id { get; }
            public string FirstName { get; }
            public string LastName { get; }
            public string CompanyName { get; }
            public string Email { get; }
            public string PhoneNumber { get; }
            public string BankAccountNumber { get; }
            public string Note { get; }
            public string IdentifyingNumber { get; }
            public string TaxIdentifyingNumber { get; }

            public EntityViewModel(LegalEntity entity)
            {
                this.Id = entity.Id;
                this.FirstName = entity.FirstName;
                this.LastName = entity.LastName;
                this.CompanyName = entity.CompanyName;
                this.Email = entity.Email;
                this.PhoneNumber = entity.PhoneNumber;
                this.BankAccountNumber = entity.BankAccountNumber;
                this.Note = entity.Note;
                this.IdentifyingNumber = entity.IdentifyingNumber;
                this.TaxIdentifyingNumber = entity.TaxIdentifyingNumber;
            }
        }
    }

}