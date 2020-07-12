using Lendee.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Contract
{
    public class ContractsController : Controller
    {
        private readonly IContractRepository repository;

        public ContractsController(IContractRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var contracts = await repository.GetAll();

            return View(contracts);
        }
    }
}