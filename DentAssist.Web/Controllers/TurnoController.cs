using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

// Controlador para la gestión de turnos (citas): agenda, creación, edición y eliminación
namespace DentAssist.Web.Controllers
{
    public class TurnoController : Controller
    {
        // Contexto de base de datos para acceder a turnos, pacientes y odontólogos
        private readonly DentAssistContext _context;

        // Inyección del contexto por constructor
        public TurnoController(DentAssistContext context)
        {
            _context = context;
        }

        // ========================================================
        // LISTADO GENERAL DE TURNOS (para administración)
        // ========================================================
        public IActionResult Index()
        {
            // Carga todos los turnos, pacientes y odontólogos
            List<Turno> turnosDb = new List<Turno>();
            foreach (Turno t in _context.Turnos)
                turnosDb.Add(t);

            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes)
                pacientes.Add(p);

            List<Odontologo> odontologos = new List<Odontologo>();
            foreach (Odontologo o in _context.Odontologo)
                odontologos.Add(o);

            // Arma la lista de turnos con datos enriquecidos (nombres de paciente y odontólogo)
            List<TurnoVista> lista = new List<TurnoVista>();
            foreach (Turno t in turnosDb)
            {
                TurnoVista tv = new TurnoVista
                {
                    Id = t.Id,
                    FechaHora = t.FechaHora,
                    Duracion = t.Duracion,
                    Estado = t.Estado,
                    Observaciones = t.Observaciones
                };

                foreach (Paciente p in pacientes)
                {
                    if (p.Id == t.PacienteId)
                    {
                        tv.PacienteNombre = p.NombreCompleto;
                        break;
                    }
                }

                foreach (Odontologo o in odontologos)
                {
                    if (o.Id == t.OdontologoId)
                    {
                        tv.OdontologoNombre = o.NombreCompleto;
                        break;
                    }
                }

                lista.Add(tv);
            }

            return View("Index", lista);
        }

        // ========================================================
        // REDIRECCIÓN A LA AGENDA DEL ODONTÓLOGO LOGUEADO
        // ========================================================
        public IActionResult MiAgenda()
        {
            // Busca el odontólogo actual por email de usuario
            string email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Index");

            int odontologoId = 0;
            foreach (Odontologo o in _context.Odontologo)
            {
                if (string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    odontologoId = o.Id;
                    break;
                }
            }

            if (odontologoId == 0)
                return RedirectToAction("Index");

            return RedirectToAction("Agenda", new { id = odontologoId });
        }

        // ========================================================
        // AGENDA SEMANAL DE UN ODONTÓLOGO (por id)
        // ========================================================
        public IActionResult Agenda(int id)
        {
            // Calcula el rango de la semana actual (lunes a domingo)
            DateTime hoy = DateTime.Today;
            int daysSinceMonday = ((int)hoy.DayOfWeek + 6) % 7;
            DateTime lunes = hoy.AddDays(-daysSinceMonday);
            DateTime domingo = lunes.AddDays(6);

            ViewData["Lunes"] = lunes;
            ViewData["Domingo"] = domingo;

            // Carga todos los turnos y pacientes
            List<Turno> turnosDb = new List<Turno>();
            foreach (Turno t in _context.Turnos)
                turnosDb.Add(t);

            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes)
                pacientes.Add(p);

            // Obtiene el nombre del odontólogo
            string odontologoNombre = "";
            foreach (Odontologo o in _context.Odontologo)
            {
                if (o.Id == id)
                {
                    odontologoNombre = o.NombreCompleto;
                    break;
                }
            }
            ViewBag.OdontologoNombre = odontologoNombre;

            // Arma lista de todos los turnos de este odontólogo
            List<TurnoVista> sinFiltro = new List<TurnoVista>();
            foreach (Turno t in turnosDb)
            {
                if (t.OdontologoId == id)
                {
                    TurnoVista tv = new TurnoVista
                    {
                        Id = t.Id,
                        FechaHora = t.FechaHora,
                        Duracion = t.Duracion,
                        Estado = t.Estado,
                        Observaciones = t.Observaciones
                    };

                    foreach (Paciente p in pacientes)
                    {
                        if (p.Id == t.PacienteId)
                        {
                            tv.PacienteNombre = p.NombreCompleto;
                            break;
                        }
                    }
                    tv.OdontologoNombre = odontologoNombre;
                    sinFiltro.Add(tv);
                }
            }
            ViewData["TurnosSinFiltro"] = sinFiltro;

            // Filtra solo los turnos de la semana actual
            List<TurnoVista> lista = new List<TurnoVista>();
            foreach (TurnoVista tv in sinFiltro)
            {
                if (tv.FechaHora.Date >= lunes && tv.FechaHora.Date <= domingo)
                    lista.Add(tv);
            }

            return View("Agenda", lista);
        }

        // ========================================================
        // CREAR NUEVO TURNO (GET y POST)
        // ========================================================
        public IActionResult Create()
        {
            CargarDropdowns(null, null, null);
            return View(new Turno { FechaHora = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Turno model)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Odontologo");

            if (!ModelState.IsValid)
            {
                CargarDropdowns(model.PacienteId, model.OdontologoId, model.Estado);
                return View(model);
            }

            _context.Turnos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ========================================================
        // EDITAR TURNO (GET y POST)
        // ========================================================
        public IActionResult Edit(int id)
        {
            Turno turno = _context.Turnos.Find(id);
            if (turno == null) return NotFound();

            CargarDropdowns(turno.PacienteId, turno.OdontologoId, turno.Estado);
            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Turno model)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Odontologo");

            if (!ModelState.IsValid)
            {
                CargarDropdowns(model.PacienteId, model.OdontologoId, model.Estado);
                return View(model);
            }

            Turno t = _context.Turnos.Find(id);
            if (t == null) return NotFound();

            // Actualiza todos los campos editables
            t.PacienteId = model.PacienteId;
            t.OdontologoId = model.OdontologoId;
            t.FechaHora = model.FechaHora;
            t.Duracion = model.Duracion;
            t.Estado = model.Estado;
            t.Observaciones = model.Observaciones;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ========================================================
        // ELIMINAR TURNO (GET y POST)
        // ========================================================
        public IActionResult Delete(int id)
        {
            Turno t = _context.Turnos.Find(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Turno t = _context.Turnos.Find(id);
            if (t != null)
            {
                _context.Turnos.Remove(t);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ========================================================
        // MÉTODO DE APOYO: CARGAR COMBOS PARA LOS FORMULARIOS
        // ========================================================
        private void CargarDropdowns(int? pacienteSeleccionado, int? odontologoSeleccionado, EstadoTurno? estadoSeleccionado)
        {
            // Combo de pacientes
            var pacientes = new List<SelectListItem>();
            foreach (Paciente p in _context.Pacientes)
            {
                pacientes.Add(new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.NombreCompleto,
                    Selected = (pacienteSeleccionado.HasValue && p.Id == pacienteSeleccionado.Value)
                });
            }

            // Combo de odontólogos
            var odontologos = new List<SelectListItem>();
            foreach (Odontologo o in _context.Odontologo)
            {
                odontologos.Add(new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.NombreCompleto,
                    Selected = (odontologoSeleccionado.HasValue && o.Id == odontologoSeleccionado.Value)
                });
            }

            // Combo de estados
            var estados = new List<SelectListItem>();
            foreach (EstadoTurno e in Enum.GetValues(typeof(EstadoTurno)))
            {
                estados.Add(new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = (estadoSeleccionado.HasValue && e == estadoSeleccionado.Value)
                });
            }

            ViewBag.Pacientes = pacientes;
            ViewBag.Odontologos = odontologos;
            ViewBag.Estados = estados;
        }
    }
}
