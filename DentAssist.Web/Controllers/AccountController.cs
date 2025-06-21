using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signIn;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<AccountController> logger)
        {
            _signIn = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Si ya está autenticado, redirijo según rol
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Odontologo"))
                    return RedirectToAction("Home");
                if (User.IsInRole("Recepcionista"))
                    return RedirectToAction("Index", "Recepcionistas");
                if (User.IsInRole("Administrador"))
                    return RedirectToAction("Index", "Landing");
                // por defecto
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido en Login");
                return View(vm);
            }

            var result = await _signIn.PasswordSignInAsync(
                vm.Email, vm.Password, vm.RememberMe, lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                _logger.LogInformation("Usuario {Email} inició sesión.", vm.Email);

                // redirijo según rol
                var user = await _userManager.FindByEmailAsync(vm.Email);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Odontologo"))
                    return RedirectToAction("Agenda", "Turno");
                if (roles.Contains("Recepcionista"))
                    return RedirectToAction("Index", "Recepcionistas");
                if (roles.Contains("Administrador"))
                    return RedirectToAction("Index", "Landing");

                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Usuario {Email} bloqueado.", vm.Email);
                return View("Lockout");
            }

            ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
            _logger.LogWarning("Inicio de sesión fallido para {Email}.", vm.Email);
            return View(vm);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido en Register");
                return View(vm);
            }

            var user = new IdentityUser { UserName = vm.Email, Email = vm.Email };
            var createResult = await _userManager.CreateAsync(user, vm.Password);
            if (createResult.Succeeded)
            {
                _logger.LogInformation("Usuario registrado: {Email}", vm.Email);
                await _signIn.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Usuario {Email} inició sesión tras registro.", vm.Email);
                return RedirectToLocal(returnUrl);
            }

            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogWarning("Error en Register: {Error}", error.Description);
            }

            return View(vm);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            await _signIn.SignOutAsync();
            _logger.LogInformation("Usuario {User} cerró sesión.", userName);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
