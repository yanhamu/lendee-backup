using Lendee.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly IContractRepository contractRepository;

        public DashboardController(IContractRepository contractRepository)
        {
            this.contractRepository = contractRepository;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await contractRepository.GetAll();
            return View(contracts);
        }
    }
}
