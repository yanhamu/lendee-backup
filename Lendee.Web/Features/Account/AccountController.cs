using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Account
{
    public class AccountController : Controller
    {
        private readonly Service service;

        public AccountController(Service service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsModel credentials, string returnUrl)
        {
            var loginResult = await service.Login(credentials);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register(string invitationId)
        {
            ViewData["invitationId"] = invitationId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCredentialsModel credentials)
        {
            var registrationResult = await service.Register(credentials);
            return LocalRedirect("/");
        }
    }
}