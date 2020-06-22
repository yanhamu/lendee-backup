using Microsoft.AspNetCore.Mvc;

namespace Lendee.Web.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Identity.IsAuthenticated
                ? (IActionResult)RedirectToAction("Index", "Dashboard")
                : View();
            }

            return View();
        }
    }
}
