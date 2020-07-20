using Lendee.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Payment
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentRepository repository;

        public PaymentsController(IPaymentRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var payments = await repository.GetLast(100, 0);
            return View(payments);
        }
    }
}