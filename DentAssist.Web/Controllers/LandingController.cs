using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Web.Controllers
{
    [AllowAnonymous]
    public class LandingController : Controller
    {
        // GET: /
        public IActionResult Index()
        {
            // Si ya está autenticado, lo mando al panel según su rol
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Recepcionista"))
                {
                    return RedirectToAction("Index", "Home");
                }

                if (User.IsInRole("Odontologo"))
                {
                    // Aquí suponemos que el odontólogo empieza en "MisPlanes"
                    return RedirectToAction("Index", "Home");
                }

                if (User.IsInRole("Administrador"))
                {
                    // Al admin le dejamos su vista de Pacientes por defecto
                    return RedirectToAction("Index", "Home");
                }
            }

            // Si no está autenticado, muestro la Landing normal
            return View();
        }
    }
}
