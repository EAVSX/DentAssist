using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    // Controlador para gestión de planes de tratamiento (solo por odontólogo autenticado)
    public class PlanesController : Controller
    {
        // Contexto de base de datos
        private readonly DentAssistContext _context;

        // Inyección de contexto
        public PlanesController(DentAssistContext context)
        {
            _context = context;
        }

        // ===========================================================
        // VER TODOS MIS PLANES (sólo los del odontólogo autenticado)
        // ===========================================================
        public IActionResult MisPlanes()
        {
            // Obtiene el ID del odontólogo actual usando su email de usuario
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

            // Obtiene todos los pacientes asociados a este odontólogo
            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes.ToList())
            {
                pacientes.Add(p);
            }

            // Guarda solo los IDs de mis pacientes
            List<int> misPacientes = new List<int>();
            foreach (Paciente p in pacientes)
            {
                if (p.OdontologoId == odontoId)
                    misPacientes.Add(p.Id);
            }

            // Trae todos los planes de tratamiento
            List<PlanTratamiento> todosPlanes = new List<PlanTratamiento>();
            foreach (PlanTratamiento plan in _context.PlanTratamientos.ToList())
            {
                todosPlanes.Add(plan);
            }

            // Filtra solo los planes de los pacientes del odontólogo actual
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

            // Muestra solo los planes que pertenecen al odontólogo logueado
            return View("MisPlanes", lista);
        }

        // ===========================================================
        // CREAR NUEVO PLAN DE TRATAMIENTO (GET y POST)
        // ===========================================================
        public IActionResult Create()
        {
            // Carga solo los pacientes del odontólogo actual para el dropdown
            CargarPacientes();
            return View(new PlanTratamiento { FechaCreacion = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanTratamiento model)
        {
            // Ignora validación de navegación
            ModelState.Remove("Paciente");
            ModelState.Remove("Pasos");

            if (!ModelState.IsValid)
            {
                CargarPacientes();
                return View(model);
            }

            // Asigna fecha y guarda el nuevo plan
            model.FechaCreacion = DateTime.Now;
            _context.PlanTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("MisPlanes");
        }

        // ===========================================================
        // DETALLE DEL PLAN Y SU PROGRESO
        // ===========================================================
        public IActionResult Details(int id)
        {
            PlanTratamiento plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            // Carga el paciente del plan
            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);

            // Carga los pasos asociados a ese plan
            List<PasoTratamiento> pasos = new List<PasoTratamiento>();
            foreach (PasoTratamiento p in _context.PasoTratamientos.ToList())
            {
                if (p.PlanTratamientoId == id)
                    pasos.Add(p);
            }
            ViewData["Pasos"] = pasos;

            // Calcula el progreso del tratamiento
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

        // ===========================================================
        // AGREGAR PASO AL PLAN (GET y POST)
        // ===========================================================
        public IActionResult AddPaso(int id)
        {
            // Devuelve el formulario con el id del plan
            return View(new PasoTratamiento { PlanTratamientoId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPaso(PasoTratamiento model)
        {
            ModelState.Remove("PlanTratamiento");
            if (!ModelState.IsValid)
                return View(model);

            // Agrega el paso nuevo y recarga el detalle del plan
            _context.PasoTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = model.PlanTratamientoId });
        }

        
        public IActionResult ExportPdf(int id)
        {
           
            return RedirectToAction("Details", new { id });
        }

        // ===========================================================
        // MÉTODO DE APOYO: CARGAR DROPDOWN DE PACIENTES DEL ODONTÓLOGO
        // ===========================================================
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

            // Carga solo los pacientes asociados al odontólogo actual
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
