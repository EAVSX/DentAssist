// Controllers/PlanTratamientoController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using DentAssist.Web.Helpers;
using DentAssist.Models;

namespace DentAssist.Web.Controllers
{
    [Authorize(Roles = "Administrador,Odontologo")]
    public class PlanTratamientoController : Controller
    {
        private readonly DentAssistContext _context;
        private readonly IConverter _pdfConverter;
        private readonly RazorViewToString _razorRenderer;

        public PlanTratamientoController(
            DentAssistContext context,
            IConverter pdfConverter,
            RazorViewToString razorRenderer)
        {
            _context = context;
            _pdfConverter = pdfConverter;
            _razorRenderer = razorRenderer;
        }

        // GET: /PlanTratamiento
        public IActionResult Index()
        {
            var planes = _context.PlanTratamientos
                                 .ToList()
                                 .Select(plan => {
                                     plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
                                     plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);
                                     return plan;
                                 })
                                 .ToList();
            return View(planes);
        }

        // GET: /PlanTratamiento/MisPlanes
        [Authorize(Roles = "Odontologo")]
        public IActionResult MisPlanes()
        {
            var email = User.Identity.Name;
            var odon = _context.Odontologo.FirstOrDefault(o => o.Email == email);
            if (odon == null) return Forbid();

            var pacientes = _context.Pacientes
                                    .Where(p => p.OdontologoId == odon.Id)
                                    .ToList();

            var misPlanes = _context.PlanTratamientos
                .Where(pt => pacientes.Select(p => p.Id).Contains(pt.PacienteId))
                .ToList()
                .Select(pt => {
                    pt.Paciente = pacientes.First(p => p.Id == pt.PacienteId);
                    pt.Tratamiento = _context.Tratamientos.Find(pt.TratamientoId);
                    return pt;
                })
                .ToList();

            return View(misPlanes);
        }

        // GET: /PlanTratamiento/Details/5
        public IActionResult Details(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);

            var pasos = _context.PasoTratamientos
                                .Where(p => p.PlanTratamientoId == id)
                                .ToList();
            ViewData["Pasos"] = pasos;
            int hechos = pasos.Count(p => p.Estado.Equals("realizado", StringComparison.OrdinalIgnoreCase));
            ViewData["Progreso"] = pasos.Count == 0 ? 0 : (hechos * 100) / pasos.Count;

            return View(plan);
        }

        // GET: /PlanTratamiento/UpdatePasoEstado
        [HttpGet("PlanTratamiento/UpdatePasoEstado")]
        public IActionResult UpdatePasoEstado()
        {
            // Si alguien accede por GET, redirigimos al listado
            return RedirectToAction(nameof(Index));
        }

        // POST: /PlanTratamiento/UpdatePasoEstado
        [HttpPost]
        public IActionResult UpdatePasoEstado(int id, int planId, string nuevoEstado)
        {
            // Buscar el paso correspondiente
            PasoTratamiento paso = null;
            foreach (PasoTratamiento p in _context.PasoTratamientos)
            {
                if (p.Id == id)
                {
                    paso = p;
                    break;
                }
            }

            if (paso == null)
            {
                // No encontrado, puedes redirigir a Details o mostrar un error
                return RedirectToAction("Details", new { id = planId });
            }

            // Actualizar el estado
            paso.Estado = nuevoEstado;
            _context.SaveChanges();

            // Redirigir a los detalles del plan
            return RedirectToAction("Details", new { id = planId });
        }

        // GET: /PlanTratamiento/Create
        public IActionResult Create()
        {
            CargarDropdownsYPrecios();
            return View();
        }

        // POST: /PlanTratamiento/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(PlanTratamiento model)
        {
            ModelState.Remove(nameof(model.FechaCreacion));
            if (!ModelState.IsValid)
            {
                CargarDropdownsYPrecios(model.PacienteId, model.TratamientoId);
                return View(model);
            }

            var tra = _context.Tratamientos.Find(model.TratamientoId);
            model.Precio = tra?.Precio ?? 0m;
            model.FechaCreacion = DateTime.Now;

            _context.PlanTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /PlanTratamiento/Edit/5
        public IActionResult Edit(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            CargarDropdownsYPrecios(plan.PacienteId, plan.TratamientoId);
            return View(plan);
        }

        // POST: /PlanTratamiento/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PlanTratamiento model)
        {
            ModelState.Remove(nameof(model.FechaCreacion));
            if (!ModelState.IsValid)
            {
                CargarDropdownsYPrecios(model.PacienteId, model.TratamientoId);
                return View(model);
            }

            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.PacienteId = model.PacienteId;
            plan.TratamientoId = model.TratamientoId;
            plan.Precio = _context.Tratamientos.Find(model.TratamientoId)?.Precio ?? 0m;
            plan.Observaciones = model.Observaciones;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /PlanTratamiento/Delete/5
        public IActionResult Delete(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);
            return View(plan);
        }

        // POST: /PlanTratamiento/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan != null)
            {
                _context.PlanTratamientos.Remove(plan);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /PlanTratamiento/AddPaso/5
        public IActionResult AddPaso(int id)
        {
            ViewData["Estados"] = new[] { "pendiente", "realizado", "cancelado" };
            ViewData["Tratamientos"] = new SelectList(_context.Tratamientos, "Id", "Nombre");
            return View(new PasoTratamiento { PlanTratamientoId = id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddPaso(PasoTratamiento model)
        {
            // 1) Limpiamos cualquier estado previo
            ModelState.Clear();

            // 2) Recargamos los dropdowns
            ViewData["Estados"] = new[] { "pendiente", "realizado", "cancelado" };
            ViewData["Tratamientos"] = new SelectList(
                _context.Tratamientos, "Id", "Nombre", model.TratamientoId
            );

            // 3) Rellenamos Descripción y Precio desde el Tratamiento
            var tra = _context.Tratamientos.Find(model.TratamientoId);
            if (tra != null)
            {
                model.Descripcion = tra.Nombre;
                model.Precio = tra.Precio;
            }

            // 4) Intentamos guardar, y si chocamos contra PK duplicada, ajustamos Id
            _context.PasoTratamientos.Add(model);
            try
            {
                _context.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.Message;
                if (inner != null && inner.Contains("Duplicate entry") && inner.Contains("for key 'PRIMARY'"))
                {
                    // calculamos el nuevo Id = max(Id) + 1
                    int maxId = _context.PasoTratamientos.Any()
                                ? _context.PasoTratamientos.Max(p => p.Id)
                                : 0;
                    model.Id = maxId + 1;

                    // volvemos a Attachear y guardar
                    _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                }
                else
                {
                    // si es otro error, relanzamos
                    throw;
                }
            }

            return RedirectToAction(nameof(Details), new { id = model.PlanTratamientoId });
        }


        public async System.Threading.Tasks.Task<IActionResult> ExportPdf(int id)
        {
            PlanTratamiento plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);

            // Cargar los pasos
            List<PasoTratamiento> pasosUnicos = new List<PasoTratamiento>();
            foreach (PasoTratamiento p in _context.PasoTratamientos)
            {
                if (p.PlanTratamientoId == id)
                {
                    bool existe = false;
                    foreach (PasoTratamiento existente in pasosUnicos)
                    {
                        if (existente.Id == p.Id)
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe)
                    {
                        pasosUnicos.Add(p);
                    }
                }
            }
            plan.Pasos = pasosUnicos;

            // Sumar los precios
            decimal sumaPrecios = 0m;
            foreach (PasoTratamiento paso in plan.Pasos)
            {
                sumaPrecios += paso.Precio;
            }
            ViewData["SumaPrecios"] = sumaPrecios;

            string html = await _razorRenderer.RenderViewAsync("ExportPdf", plan);

            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    DocumentTitle = $"Plan_{plan.Id}"
                },
                Objects = {
            new ObjectSettings
            {
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" }
            }
        }
            };

            byte[] pdf = _pdfConverter.Convert(doc);
            return File(pdf, "application/pdf", $"Plan_{plan.Id}.pdf");
        }


        private void CargarDropdownsYPrecios(int? pacienteId = null, int? tratamientoId = null)
        {
            ViewData["Pacientes"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto", pacienteId);
            ViewData["Tratamientos"] = new SelectList(_context.Tratamientos, "Id", "Nombre", tratamientoId);
            var dict = _context.Tratamientos.ToDictionary(t => t.Id, t => t.Precio);
            ViewData["PreciosTratamientos"] = dict;
        }
    }
}
