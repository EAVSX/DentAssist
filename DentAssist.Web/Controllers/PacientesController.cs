using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    // Controlador encargado de CRUD para pacientes
    public class PacienteController : Controller
    {
        // Contexto de base de datos para acceder a los pacientes y odontólogos
        private readonly DentAssistContext _context;

        // Inyección del contexto por constructor
        public PacienteController(DentAssistContext context)
        {
            _context = context;
        }

        // ========================================================
        // LISTADO DE PACIENTES
        // ========================================================
        // Muestra todos los pacientes registrados
        public IActionResult Index()
        {
            var lista = new List<Paciente>();
            foreach (var p in _context.Pacientes)
                lista.Add(p);
            return View(lista);
        }

        // ========================================================
        // CREAR PACIENTE (GET y POST)
        // ========================================================
        // Formulario de creación de paciente, carga odontólogos para el dropdown
        public IActionResult Create()
        {
            ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto");
            return View();
        }

        // Procesa la creación de paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente model)
        {
            // Remueve validaciones de navegación que no interesan en este formulario
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

        // ========================================================
        // EDITAR PACIENTE (GET y POST)
        // ========================================================
        // Formulario de edición, carga los odontólogos actuales para el dropdown
        public IActionResult Edit(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null) return NotFound();

            ViewBag.Odontologos = new SelectList(_context.Odontologo, "Id", "NombreCompleto", paciente.OdontologoId);
            return View(paciente);
        }

        // Procesa los cambios al editar un paciente
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

            // Actualiza los campos principales
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

        // ========================================================
        // ELIMINAR PACIENTE (GET y POST)
        // ========================================================
        // Muestra la confirmación antes de eliminar
        public IActionResult Delete(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // Confirma y ejecuta la eliminación
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
