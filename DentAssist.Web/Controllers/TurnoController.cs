using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    public class TurnoController : Controller
    {
        private readonly DentAssistContext _context;

        public TurnoController(DentAssistContext context)
        {
            _context = context;
        }

        // GET: /Turno
        public IActionResult Index()
        {
            List<Turno> turnosDb = new List<Turno>();
            foreach (Turno t in _context.Turnos.ToList())
                turnosDb.Add(t);

            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes.ToList())
                pacientes.Add(p);

            List<Odontologo> odontologos = new List<Odontologo>();
            foreach (Odontologo o in _context.Odontologo.ToList())
                odontologos.Add(o);

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

        // GET: /Turno/MiAgenda
        public IActionResult MiAgenda()
        {
            string email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Index");

            int odontologoId = 0;
            foreach (Odontologo o in _context.Odontologo.ToList())
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

        // GET: /Turno/Agenda/5
        public IActionResult Agenda(int id)
        {
            DateTime hoy = DateTime.Today;
            int daysSinceMonday = ((int)hoy.DayOfWeek + 6) % 7;
            DateTime lunes = hoy.AddDays(-daysSinceMonday);
            DateTime domingo = lunes.AddDays(6);

            ViewData["Lunes"] = lunes;
            ViewData["Domingo"] = domingo;

            List<Turno> turnosDb = new List<Turno>();
            foreach (Turno t in _context.Turnos.ToList())
                turnosDb.Add(t);

            List<Paciente> pacientes = new List<Paciente>();
            foreach (Paciente p in _context.Pacientes.ToList())
                pacientes.Add(p);

            string odontologoNombre = "";
            foreach (Odontologo o in _context.Odontologo.ToList())
            {
                if (o.Id == id)
                {
                    odontologoNombre = o.NombreCompleto;
                    break;
                }
            }
            ViewBag.OdontologoNombre = odontologoNombre;

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

            List<TurnoVista> lista = new List<TurnoVista>();
            foreach (TurnoVista tv in sinFiltro)
            {
                if (tv.FechaHora.Date >= lunes && tv.FechaHora.Date <= domingo)
                    lista.Add(tv);
            }

            return View("Agenda", lista);
        }

        // GET: /Turno/Create
        public IActionResult Create()
        {
            CargarDropdowns(null, null, null);
            return View(new Turno { FechaHora = DateTime.Now });
        }

        // POST: /Turno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Turno model)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Odontologo");

            if (!ModelState.IsValid)
            {
                // PASA LOS SELECCIONADOS
                CargarDropdowns(model.PacienteId, model.OdontologoId, model.Estado);
                return View(model);
            }

            _context.Turnos.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Turno/Edit/5
        public IActionResult Edit(int id)
        {
            Turno turno = _context.Turnos.Find(id);
            if (turno == null) return NotFound();

            CargarDropdowns(turno.PacienteId, turno.OdontologoId, turno.Estado);
            return View(turno);
        }

        // POST: /Turno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Turno model)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Odontologo");

            if (!ModelState.IsValid)
            {
                // PASA LOS SELECCIONADOS
                CargarDropdowns(model.PacienteId, model.OdontologoId, model.Estado);
                return View(model);
            }

            Turno t = _context.Turnos.Find(id);
            if (t == null) return NotFound();

            t.PacienteId = model.PacienteId;
            t.OdontologoId = model.OdontologoId;
            t.FechaHora = model.FechaHora;
            t.Duracion = model.Duracion;
            t.Estado = model.Estado;
            t.Observaciones = model.Observaciones;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Turno/Delete/5
        public IActionResult Delete(int id)
        {
            Turno t = _context.Turnos.Find(id);
            if (t == null) return NotFound();
            return View(t);
        }

        // POST: /Turno/Delete/5
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

        // AJUSTADO: Cargar combos con valor seleccionado
        private void CargarDropdowns(int? pacienteSeleccionado, int? odontologoSeleccionado, EstadoTurno? estadoSeleccionado)
        {
            var pacientes = new List<SelectListItem>();
            foreach (Paciente p in _context.Pacientes.ToList())
                pacientes.Add(new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.NombreCompleto,
                    Selected = (pacienteSeleccionado.HasValue && p.Id == pacienteSeleccionado.Value)
                });

            var odontologos = new List<SelectListItem>();
            foreach (Odontologo o in _context.Odontologo.ToList())
                odontologos.Add(new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.NombreCompleto,
                    Selected = (odontologoSeleccionado.HasValue && o.Id == odontologoSeleccionado.Value)
                });

            var estados = new List<SelectListItem>();
            foreach (EstadoTurno e in Enum.GetValues(typeof(EstadoTurno)))
                estados.Add(new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = (estadoSeleccionado.HasValue && e == estadoSeleccionado.Value)
                });

            ViewBag.Pacientes = pacientes;
            ViewBag.Odontologos = odontologos;
            ViewBag.Estados = estados;
        }
    }
}
