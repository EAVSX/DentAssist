// Controlador para gestión de recepcionistas: CRUD, estadísticas y usuarios Identity
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Datos;
using DentAssist.Web.Models; // Turno y EstadoTurno
using DentAssist.Models;     // Recepcionista

namespace DentAssist.Web.Controllers
{
    // Solo administradores pueden gestionar recepcionistas
    [Authorize(Roles = "Administrador")]
    public class RecepcionistasController : Controller
    {
        // Dependencias: contexto de datos y servicios de usuarios y roles
        private readonly DentAssistContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Inyección de dependencias por constructor
        public RecepcionistasController(
            DentAssistContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ========================================================
        // PANEL PRINCIPAL Y LISTADO DE RECEPCIONISTAS
        // ========================================================
        public IActionResult Index()
        {
            // Estadísticas para el panel (dashboard)
            ViewBag.TotalPacientes = 0;
            foreach (var p in _context.Pacientes) ViewBag.TotalPacientes++;

            int turnosProgramados = 0, turnosHoy = 0;
            foreach (Turno t in _context.Turnos)
            {
                if (t.Estado == EstadoTurno.Programado) turnosProgramados++;
                if (t.FechaHora.Date == DateTime.Today) turnosHoy++;
            }
            ViewBag.TurnosProgramados = turnosProgramados;
            ViewBag.TurnosHoy = turnosHoy;

            // Listado tradicional de recepcionistas
            List<Recepcionista> lista = new List<Recepcionista>();
            foreach (Recepcionista r in _context.Recepcionistas)
            {
                lista.Add(r);
            }

            return View(lista);
        }

        // ========================================================
        // CREAR NUEVO RECEPCIONISTA (GET y POST)
        // ========================================================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Create(Recepcionista model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Guarda el recepcionista en la base relacional
            _context.Recepcionistas.Add(model);
            _context.SaveChanges();

            // Crea el usuario en Identity
            IdentityUser user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            IdentityResult creation = await _userManager.CreateAsync(user, model.Password);
            if (!creation.Succeeded)
            {
                // Si falla Identity, elimina el recepcionista (rollback manual)
                foreach (var err in creation.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                _context.Recepcionistas.Remove(model);
                _context.SaveChanges();
                return View(model);
            }

            // Asegura que exista el rol "Recepcionista" y lo asigna al usuario
            bool exists = await _roleManager.RoleExistsAsync("Recepcionista");
            if (!exists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Recepcionista"));
            }
            await _userManager.AddToRoleAsync(user, "Recepcionista");

            return RedirectToAction("Index");
        }

        // ========================================================
        // ELIMINAR RECEPCIONISTA (GET y POST)
        // ========================================================
        public IActionResult Delete(int id)
        {
            Recepcionista r = _context.Recepcionistas.Find(id);
            if (r == null) return NotFound();
            return View(r);
        }

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
