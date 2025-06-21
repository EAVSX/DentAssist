using Microsoft.AspNetCore.Mvc;
using DentAssist.Models;
using Microsoft.AspNetCore.Authorization;
using DentAssist.Web.Datos;

namespace DentAssist.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionController : Controller
    {
        private readonly DentAssistContext _contexto;

        public ConfiguracionController(DentAssistContext contexto)
        {
            _contexto = contexto;
        }

        // Mostrar la configuración (solo un registro)
        public IActionResult Index()
        {
            Configuracion conf = null;
            foreach (Configuracion c in _contexto.Configuraciones)
            {
                conf = c;
                break;
            }
            if (conf == null)
            {
                conf = new Configuracion();
            }
            return View(conf);
        }

        // Guardar cambios de la configuración
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Configuracion configuracion)
        {
            if (ModelState.IsValid)
            {
                Configuracion conf = null;
                foreach (Configuracion c in _contexto.Configuraciones)
                {
                    conf = c;
                    break;
                }
                if (conf == null)
                {
                    // Si no existe, crea nueva
                    _contexto.Configuraciones.Add(configuracion);
                }
                else
                {
                    // Si existe, actualiza
                    conf.NombreClinica = configuracion.NombreClinica;
                    conf.Direccion = configuracion.Direccion;
                    conf.Telefono = configuracion.Telefono;
                    conf.EmailContacto = configuracion.EmailContacto;
                }
                _contexto.SaveChanges();
                ViewBag.Mensaje = "Configuración guardada correctamente.";
                return View(configuracion);
            }
            return View(configuracion);
        }
    }
}
