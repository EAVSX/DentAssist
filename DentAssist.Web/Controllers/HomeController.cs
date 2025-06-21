using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DentAssist.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IdentityUser usuario = await _userManager.GetUserAsync(User);
            if (usuario != null)
            {
                // Verifica el rol y redirige a la vista correspondiente
                if (await _userManager.IsInRoleAsync(usuario, "Administrador"))
                {
                    return View("index");
                }
                else if (await _userManager.IsInRoleAsync(usuario, "Odontólogo"))
                {
                    return View("OdontologoPanel");
                }
                else if (await _userManager.IsInRoleAsync(usuario, "Recepcionista"))
                {
                    return View("RecepcionistaPanel");
                }
            }
            // Si no está autenticado, muestra algo básico
            return View("Landing");
        }
    }
}
