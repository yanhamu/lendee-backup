using Microsoft.AspNetCore.Mvc;

namespace Lendee.Web.Features.Dashboard
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
