// Controllers/RecepcionistasController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;      // Para Turno y EstadoTurno
using DentAssist.Models;          // Para Recepcionista

namespace DentAssist.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RecepcionistasController : Controller
    {
        private readonly DentAssistContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RecepcionistasController(
            DentAssistContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Recepcionistas
        public IActionResult Index()
        {
            // ──────────────────────────────────────────────
            // 1) Estadísticas para el panel (ViewBag)
            // ──────────────────────────────────────────────
            ViewBag.TotalPacientes = _context.Pacientes.Count();
            ViewBag.TurnosProgramados = _context.Turnos.Count(t => t.Estado == EstadoTurno.Programado);
            ViewBag.TurnosHoy = _context.Turnos.Count(t => t.FechaHora.Date == DateTime.Today);

            // ──────────────────────────────────────────────
            // 2) Listado de recepcionistas (sin cambios)
            // ──────────────────────────────────────────────
            List<Recepcionista> lista = new List<Recepcionista>();
            foreach (Recepcionista r in _context.Recepcionistas)
            {
                lista.Add(r);
            }

            return View(lista);
        }

        // GET: /Recepcionistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Recepcionistas/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recepcionista model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1) Guardar en tu tabla Recepcionista
            _context.Recepcionistas.Add(model);
            _context.SaveChanges();

            // 2) Crear el usuario en Identity con la misma contraseña
            IdentityUser user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            IdentityResult creation = await _userManager.CreateAsync(user, model.Password);
            if (!creation.Succeeded)
            {
                // Si falla, rollback de la entidad
                foreach (var err in creation.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                _context.Recepcionistas.Remove(model);
                _context.SaveChanges();
                return View(model);
            }

            // 3) Asegurar existencia del rol
            bool exists = await _roleManager.RoleExistsAsync("Recepcionista");
            if (!exists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Recepcionista"));
            }

            // 4) Asignar rol
            await _userManager.AddToRoleAsync(user, "Recepcionista");

            return RedirectToAction("Index");
        }

        // GET: /Recepcionistas/Delete/5
        public IActionResult Delete(int id)
        {
            Recepcionista r = _context.Recepcionistas.Find(id);
            if (r == null) return NotFound();
            return View(r);
        }

        // POST: /Recepcionistas/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Recepcionista r = _context.Recepcionistas.Find(id);
            if (r != null)
            {
                _context.Recepcionistas.Remove(r);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
