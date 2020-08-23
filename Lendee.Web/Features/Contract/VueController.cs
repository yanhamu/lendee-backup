using Microsoft.AspNetCore.Mvc;

namespace Lendee.Web.Features.Contract
{
    public class VueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
