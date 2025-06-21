using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    public class PacienteController : Controller
    {
        private readonly DentAssistContext _context;

        public PacienteController(DentAssistContext context)
        {
            _context = context;
        }

        // GET: /Paciente
        public IActionResult Index()
        {
            var lista = new List<Paciente>();
            foreach (var p in _context.Pacientes)
                lista.Add(p);
            return View(lista);
        }

        // GET: /Paciente/Create
        public IActionResult Create()
        {
            // cargamos dropdown de odontólogos (si quieres mantenerlo, o comenta la línea para quitarlo)
            ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto");
            return View();
        }

        // POST: /Paciente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente model)
        {
            // Quitamos toda validación de colecciones y navegación
            ModelState.Remove("Turnos");
            ModelState.Remove("Planes");
            ModelState.Remove("Odontologo");
            ModelState.Remove("OdontologoId");

            if (!ModelState.IsValid)
            {
                ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto", model.OdontologoId);
                return View(model);
            }

            _context.Pacientes.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Paciente/Edit/5
        public IActionResult Edit(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null) return NotFound();

            ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto", paciente.OdontologoId);
            return View(paciente);
        }

        // POST: /Paciente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Paciente model)
        {
            ModelState.Remove("Turnos");
            ModelState.Remove("Planes");
            ModelState.Remove("Odontologo");
            ModelState.Remove("OdontologoId");

            if (!ModelState.IsValid)
            {
                ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto", model.OdontologoId);
                return View(model);
            }

            var paciente = _context.Pacientes.Find(id);
            if (paciente == null) return NotFound();

            paciente.Nombre = model.Nombre;
            paciente.Apellido = model.Apellido;
            paciente.Rut = model.Rut;
            paciente.FechaNacimiento = model.FechaNacimiento;
            paciente.Telefono = model.Telefono;
            paciente.Email = model.Email;
            paciente.Direccion = model.Direccion;
            paciente.OdontologoId = model.OdontologoId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Paciente/Delete/5
        public IActionResult Delete(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // POST: /Paciente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
