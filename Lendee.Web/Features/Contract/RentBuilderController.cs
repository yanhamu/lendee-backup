using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class RentBuilderController : Controller
    {
        private readonly IContractRepository contractRepository;
        private readonly IContractDraftRepository draftRepository;
        public RentBuilderController(
            IContractRepository contractRepository,
            IContractDraftRepository draftRepository)
        {
            this.contractRepository = contractRepository;
            this.draftRepository = draftRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Rent(long contractId)
        {
            var rent = await contractRepository.FindRent(contractId);
            return View("Rent", new RentViewModel(rent.Type)
            {
                ContractId = rent.Id,
                PaymentTermType = rent.PaymentTermType,
                PaymentAmount = rent.Amount,
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
            SetContract(rent, model);
            rent.Amount = model.PaymentAmount;
            await contractRepository.Save();
            return await IncreaseDraftStepAndRedirect(model.ContractId);
        }

        [HttpGet]
        public async Task<IActionResult> VariableRent(long contractId)
        {
            var rent = await contractRepository.FindVariableRent(contractId);
            return View("Rent", new RentViewModel(rent.Type)
            {
                ContractId = rent.Id,
                PaymentTermType = rent.PaymentTermType,
                PaymentAmount = rent.UnitPrice,
                ValidUntil = rent.ValidUntil,
                ValidFrom = DateTime.Now,
                Day = rent.PaymentTermData?.Day,
                Month = rent.PaymentTermData?.Month
            });
        }

        [HttpPost]
        public async Task<IActionResult> VariableRent(RentViewModel model)
        {
            var rent = await contractRepository.FindVariableRent(model.ContractId);
            SetContract(rent, model);
            rent.UnitPrice = model.PaymentAmount;
            await contractRepository.Save();
            return await IncreaseDraftStepAndRedirect(model.ContractId);
        }

        [HttpGet]
        public async Task<IActionResult> CombinedRent(long contractId)
        {
            var rent = await contractRepository.FindCombinedRent(contractId);
            return View("Rent", new RentViewModel(rent.Type)
            {
                ContractId = rent.Id,
                PaymentTermType = rent.PaymentTermType,
                PaymentAmount = rent.Amount,
                Fee = rent.Fee,
                ValidUntil = rent.ValidUntil,
                ValidFrom = DateTime.Now,
                Day = rent.PaymentTermData?.Day,
                Month = rent.PaymentTermData?.Month
            });
        }

        [HttpPost]
        public async Task<IActionResult> CombinedRent(RentViewModel model)
        {
            var rent = await contractRepository.FindCombinedRent(model.ContractId);
            SetContract(rent, model);
            rent.Amount = model.PaymentAmount;
            rent.Fee = model.Fee;
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

        private void SetContract(Core.Domain.Model.Contract contract, RentViewModel model)
        {
            contract.PaymentTermType = model.PaymentTermType;
            contract.ValidFrom = model.ValidFrom;
            contract.ValidUntil = model.ValidUntil;
            contract.PaymentTermData = new PaymentTerm()
            {
                Day = model.Day,
                Month = model.Month
            };
        }

        public class RentViewModel
        {
            public long ContractId { get; set; }
            public ContractType ContractType { get; set; }
            public decimal PaymentAmount { get; set; }
            public decimal Fee { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public PaymentTermType PaymentTermType { get; set; }
            public int? Day { get; set; }
            public int? Month { get; set; }

            public RentViewModel() { }

            public RentViewModel(ContractType contractType)
            {
                this.ContractType = contractType;
            }
        }
    }
}
