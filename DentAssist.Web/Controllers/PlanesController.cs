using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    public class PlanesController : Controller
    {
        private readonly DentAssistContext _context;

        public PlanesController(DentAssistContext context)
        {
            _context = context;
        }

        // GET: /Planes/MisPlanes
        public IActionResult MisPlanes()
        {
            // 1. Obtener odontólogo actual por email
            string email = User.Identity?.Name;
            int odontoId = 0;
            foreach (Odontologo o in _context.Odontologo.ToList())
            {
                if (string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    odontoId = o.Id;
                    break;
                }
            }

            // 2. Cargar pacientes y filtrar por OdontologoId
            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes.ToList())
            {
                pacientes.Add(p);
            }

            List<int> misPacientes = new List<int>();
            foreach (Paciente p in pacientes)
            {
                if (p.OdontologoId == odontoId)
                    misPacientes.Add(p.Id);
            }

            // 3. Cargar todos los planes
            List<PlanTratamiento> todosPlanes = new List<PlanTratamiento>();
            foreach (PlanTratamiento plan in _context.PlanTratamientos.ToList())
            {
                todosPlanes.Add(plan);
            }

            // 4. Filtrar solo los de mis pacientes
            List<PlanTratamiento> lista = new List<PlanTratamiento>();
            foreach (PlanTratamiento plan in todosPlanes)
            {
                foreach (int pid in misPacientes)
                {
                    if (plan.PacienteId == pid)
                    {
                        lista.Add(plan);
                        break;
                    }
                }
            }

            return View("MisPlanes", lista);
        }

        // GET: /Planes/Create
        public IActionResult Create()
        {
            // Cargar dropdown de solo mis pacientes
            CargarPacientes();
            return View(new PlanTratamiento { FechaCreacion = DateTime.Now });
        }

        // POST: /Planes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanTratamiento model)
        {
            // Ignorar navegación
            ModelState.Remove("Paciente");
            ModelState.Remove("Pasos");

            if (!ModelState.IsValid)
            {
                CargarPacientes();
                return View(model);
            }

            model.FechaCreacion = DateTime.Now;
            _context.PlanTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("MisPlanes");
        }

        // GET: /Planes/Details/5
        public IActionResult Details(int id)
        {
            PlanTratamiento plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            // Cargar paciente
            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);

            // Cargar pasos
            List<PasoTratamiento> pasos = new List<PasoTratamiento>();
            foreach (PasoTratamiento p in _context.PasoTratamientos.ToList())
            {
                if (p.PlanTratamientoId == id)
                    pasos.Add(p);
            }
            ViewData["Pasos"] = pasos;

            // Calcular progreso
            int total = 0, hechos = 0;
            foreach (PasoTratamiento p in pasos)
            {
                total++;
                if (string.Equals(p.Estado, "realizado", StringComparison.OrdinalIgnoreCase))
                    hechos++;
            }
            int progreso = (total > 0) ? (hechos * 100 / total) : 0;
            ViewData["Progreso"] = progreso;

            return View(plan);
        }

        // GET: /Planes/AddPaso/5
        public IActionResult AddPaso(int id)
        {
            return View(new PasoTratamiento { PlanTratamientoId = id });
        }

        // POST: /Planes/AddPaso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPaso(PasoTratamiento model)
        {
            ModelState.Remove("PlanTratamiento");
            if (!ModelState.IsValid)
                return View(model);

            _context.PasoTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = model.PlanTratamientoId });
        }

        // GET: /Planes/ExportPdf/5
        public IActionResult ExportPdf(int id)
        {
            // Aquí deberías generar el PDF (ej: Rotativa, SelectPdf, iTextSharp…)
            // Por ahora devolvemos la vista Details para descarga manual:
            return RedirectToAction("Details", new { id });
        }

        // Helper: cargar dropdown de pacientes del odontólogo
        private void CargarPacientes()
        {
            string email = User.Identity?.Name;
            int odontoId = 0;
            foreach (Odontologo o in _context.Odontologo.ToList())
            {
                if (string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    odontoId = o.Id;
                    break;
                }
            }

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Paciente p in _context.Pacientes.ToList())
            {
                if (p.OdontologoId == odontoId)
                {
                    items.Add(new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NombreCompleto
                    });
                }
            }

            ViewBag.Pacientes = items;
        }
    }
}
