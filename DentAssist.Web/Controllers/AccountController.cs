using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    // Controlador de cuentas de usuario: login, registro, logout y acceso denegado
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // Servicios principales para autenticación y gestión de usuarios
        private readonly SignInManager<IdentityUser> _signIn;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        // Inyección de dependencias para login, usuarios y logs
        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<AccountController> logger)
        {
            _signIn = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        // ==========================================
        // LOGIN (GET y POST)
        // ==========================================

        // Muestra el formulario de login. Si ya está autenticado, redirige según el rol.
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirección automática según rol del usuario ya autenticado
                if (User.IsInRole("Odontologo"))
                    return RedirectToAction("Home");
                if (User.IsInRole("Recepcionista"))
                    return RedirectToAction("Index", "Recepcionistas");
                if (User.IsInRole("Administrador"))
                    return RedirectToAction("Index", "Landing");
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Procesa los datos del login. Valida credenciales, registra logs, redirige según rol.
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
                // Login correcto: redirige según el primer rol encontrado
                _logger.LogInformation("Usuario {Email} inició sesión.", vm.Email);
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
                // Usuario bloqueado tras intentos fallidos
                _logger.LogWarning("Usuario {Email} bloqueado.", vm.Email);
                return View("Lockout");
            }

            // Credenciales incorrectas
            ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
            _logger.LogWarning("Inicio de sesión fallido para {Email}.", vm.Email);
            return View(vm);
        }

        // ==========================================
        // REGISTRO (GET y POST)
        // ==========================================

        // Muestra el formulario de registro de usuario
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Procesa el registro, crea usuario y lo inicia sesión si es correcto
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
                // Usuario creado y logueado automáticamente
                _logger.LogInformation("Usuario registrado: {Email}", vm.Email);
                await _signIn.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Usuario {Email} inició sesión tras registro.", vm.Email);
                return RedirectToLocal(returnUrl);
            }

            // Si hubo errores en la creación, se muestran
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogWarning("Error en Register: {Error}", error.Description);
            }

            return View(vm);
        }

        // ==========================================
        // LOGOUT (POST)
        // ==========================================

        // Cierra la sesión y redirige al login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            await _signIn.SignOutAsync();
            _logger.LogInformation("Usuario {User} cerró sesión.", userName);
            return RedirectToAction("Login", "Account");
        }

        // ==========================================
        // ACCESO DENEGADO
        // ==========================================

        // Muestra la vista de acceso denegado si el usuario intenta acceder a un recurso prohibido
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ==========================================
        // MÉTODO DE APOYO: REDIRECCIÓN SEGURA
        // ==========================================

        // Si la URL de retorno es válida y local, redirige allí; si no, va a Home
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
