using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.DTOs;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUsersServices _usersService;

        public AccountController(IUsersServices usersService, INotyfService notifyService)
        {
            _usersService = usersService;
            _notifyService = notifyService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _usersService.LoginAsync(dto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña inconrrectos");
                _notifyService.Error("Email o contraseña inconrrectos");

                return View(dto);
            }
            return View(dto);

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
