// Controlador para la gestión de planes de tratamiento: CRUD, gestión de pasos, PDF, y vistas tanto para admin como para odontólogo
using System;
using System.Collections.Generic;
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
    // Solo usuarios con rol "Administrador" o "Odontólogo" pueden acceder
    [Authorize(Roles = "Administrador,Odontologo")]
    public class PlanTratamientoController : Controller
    {
        // Contexto de base de datos, convertidor PDF y helper para renderizar vistas como HTML
        private readonly DentAssistContext _context;
        private readonly IConverter _pdfConverter;
        private readonly RazorViewToString _razorRenderer;

        // Inyección de dependencias por constructor
        public PlanTratamientoController(
            DentAssistContext context,
            IConverter pdfConverter,
            RazorViewToString razorRenderer)
        {
            _context = context;
            _pdfConverter = pdfConverter;
            _razorRenderer = razorRenderer;
        }

        // ========================================================
        // LISTADO GENERAL DE PLANES DE TRATAMIENTO
        // ========================================================
        public IActionResult Index()
        {
            // 1) Copia planes a memoria (una sola consulta, conexión se libera)
            List<PlanTratamiento> planes = new List<PlanTratamiento>();
            foreach (PlanTratamiento p in _context.PlanTratamientos)
            {
                planes.Add(p);
            }

            // 2) Copia pacientes a memoria
            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente pac in _context.Pacientes)
            {
                pacientes.Add(pac);
            }

            // 3) Copia tratamientos a memoria
            List<Tratamiento> tratamientos = new List<Tratamiento>();
            foreach (Tratamiento tra in _context.Tratamientos)
            {
                tratamientos.Add(tra);
            }

            // 4) Relaciona cada plan con su paciente y tratamiento correspondientes
            foreach (PlanTratamiento plan in planes)
            {
                // Asigna paciente
                foreach (Paciente pac in pacientes)
                {
                    if (pac.Id == plan.PacienteId)
                    {
                        plan.Paciente = pac;
                        break;
                    }
                }

                // Asigna tratamiento
                foreach (Tratamiento tra in tratamientos)
                {
                    if (tra.Id == plan.TratamientoId)
                    {
                        plan.Tratamiento = tra;
                        break;
                    }
                }
            }

            return View(planes);
        }

        // ========================================================
        // LISTADO SOLO DEL ODONTÓLOGO AUTENTICADO
        // ========================================================
        [Authorize(Roles = "Odontologo")]
        public IActionResult MisPlanes()
        {
            // ――― 1) Obtiene el odontólogo actual
            string email = User.Identity?.Name;
            Odontologo odon = null;
            foreach (Odontologo o in _context.Odontologo)
            {
                if (o.Email == email) { odon = o; break; }
            }
            if (odon == null) return Forbid();

            // ――― 2) Materializa listas para evitar la excepción de conexión en uso
            var planesDb = new List<PlanTratamiento>();
            foreach (PlanTratamiento p in _context.PlanTratamientos) planesDb.Add(p);

            var pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes)
            {
                if (p.OdontologoId == odon.Id) pacientes.Add(p);
            }

            var tratamientos = new Dictionary<int, Tratamiento>();
            foreach (Tratamiento t in _context.Tratamientos)
            {
                tratamientos[t.Id] = t;
            }

            // ――― 3) Construye la lista final
            var misPlanes = new List<PlanTratamiento>();
            foreach (PlanTratamiento pt in planesDb)
            {
                foreach (Paciente paciente in pacientes)
                {
                    if (pt.PacienteId == paciente.Id)
                    {
                        pt.Paciente = paciente;
                        pt.Tratamiento = tratamientos.ContainsKey(pt.TratamientoId)
                                         ? tratamientos[pt.TratamientoId]
                                         : null;
                        misPlanes.Add(pt);
                        break;
                    }
                }
            }
            return View(misPlanes);
        }

        // ========================================================
        // DETALLE DE UN PLAN DE TRATAMIENTO
        // ========================================================
        public IActionResult Details(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            // Carga paciente, tratamiento y pasos asociados
            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);

            List<PasoTratamiento> pasos = new List<PasoTratamiento>();
            foreach (PasoTratamiento p in _context.PasoTratamientos)
            {
                if (p.PlanTratamientoId == id)
                    pasos.Add(p);
            }
            ViewData["Pasos"] = pasos;

            // Calcula progreso según pasos realizados
            int hechos = 0, total = pasos.Count;
            foreach (PasoTratamiento p in pasos)
            {
                if (p.Estado != null && p.Estado.Equals("realizado", StringComparison.OrdinalIgnoreCase))
                    hechos++;
            }
            ViewData["Progreso"] = total == 0 ? 0 : (hechos * 100) / total;

            return View(plan);
        }

        // ========================================================
        // ACTUALIZAR ESTADO DE UN PASO (GET: solo redirige, POST: cambia estado)
        // ========================================================
        [HttpGet("PlanTratamiento/UpdatePasoEstado")]
        public IActionResult UpdatePasoEstado()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdatePasoEstado(int id, int planId, string nuevoEstado)
        {
            // Busca el paso, lo actualiza y guarda
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
                return RedirectToAction("Details", new { id = planId });

            paso.Estado = nuevoEstado;
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = planId });
        }

        // ========================================================
        // CREAR NUEVO PLAN DE TRATAMIENTO (GET y POST)
        // ========================================================
        public IActionResult Create()
        {
            CargarDropdownsYPrecios();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(PlanTratamiento model)
        {
            ModelState.Remove(nameof(model.FechaCreacion));
            if (!ModelState.IsValid)
            {
                CargarDropdownsYPrecios(model.PacienteId, model.TratamientoId);
                return View(model);
            }
            // Asigna precio y fecha según tratamiento seleccionado
            Tratamiento tra = _context.Tratamientos.Find(model.TratamientoId);
            model.Precio = tra != null ? tra.Precio : 0m;
            model.FechaCreacion = DateTime.Now;

            _context.PlanTratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // EDITAR PLAN DE TRATAMIENTO (GET y POST)
        // ========================================================
        public IActionResult Edit(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            CargarDropdownsYPrecios(plan.PacienteId, plan.TratamientoId);
            return View(plan);
        }

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

        // ========================================================
        // ELIMINAR PLAN DE TRATAMIENTO (GET y POST)
        // ========================================================
        public IActionResult Delete(int id)
        {
            var plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);
            return View(plan);
        }

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

        // ========================================================
        // AGREGAR PASO A UN PLAN DE TRATAMIENTO (GET y POST)
        // ========================================================
        public IActionResult AddPaso(int id)
        {
            ViewData["Estados"] = new[] { "pendiente", "realizado", "cancelado" };
            ViewData["Tratamientos"] = new SelectList(_context.Tratamientos, "Id", "Nombre");
            return View(new PasoTratamiento { PlanTratamientoId = id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddPaso(PasoTratamiento model)
        {
            // Limpia y recarga dropdowns
            ModelState.Clear();
            ViewData["Estados"] = new[] { "pendiente", "realizado", "cancelado" };
            ViewData["Tratamientos"] = new SelectList(
                _context.Tratamientos, "Id", "Nombre", model.TratamientoId
            );

            // Rellena descripción y precio desde el tratamiento seleccionado
            var tra = _context.Tratamientos.Find(model.TratamientoId);
            if (tra != null)
            {
                model.Descripcion = tra.Nombre;
                model.Precio = tra.Precio;
            }

            // Intenta guardar el nuevo paso (ajusta Id si hay conflicto por PK duplicada)
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
                    int maxId = 0;
                    foreach (PasoTratamiento p in _context.PasoTratamientos)
                    {
                        if (p.Id > maxId)
                            maxId = p.Id;
                    }
                    model.Id = maxId + 1;
                    _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = model.PlanTratamientoId });
        }

        // ========================================================
        // EXPORTAR PLAN A PDF
        // ========================================================
        public async System.Threading.Tasks.Task<IActionResult> ExportPdf(int id)
        {
            // Busca el plan y carga paciente, tratamiento y pasos
            PlanTratamiento plan = _context.PlanTratamientos.Find(id);
            if (plan == null) return NotFound();

            plan.Paciente = _context.Pacientes.Find(plan.PacienteId);
            plan.Tratamiento = _context.Tratamientos.Find(plan.TratamientoId);

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
                        pasosUnicos.Add(p);
                }
            }
            plan.Pasos = pasosUnicos;

            // Suma total de precios de todos los pasos
            decimal sumaPrecios = 0m;
            foreach (PasoTratamiento paso in plan.Pasos)
            {
                sumaPrecios += paso.Precio;
            }
            ViewData["SumaPrecios"] = sumaPrecios;

            // Renderiza la vista a HTML y la convierte a PDF
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

        // ========================================================
        // MÉTODO DE APOYO: CARGA DROPDOWNS PARA FORMULARIOS
        // ========================================================
        private void CargarDropdownsYPrecios(int? pacienteId = null, int? tratamientoId = null)
        {
            ViewData["Pacientes"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto", pacienteId);
            ViewData["Tratamientos"] = new SelectList(_context.Tratamientos, "Id", "Nombre", tratamientoId);

            // Diccionario con precios de cada tratamiento
            Dictionary<int, decimal> dict = new Dictionary<int, decimal>();
            foreach (Tratamiento t in _context.Tratamientos)
            {
                dict[t.Id] = t.Precio;
            }
            ViewData["PreciosTratamientos"] = dict;
        }
    }
}
