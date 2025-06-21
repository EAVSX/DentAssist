using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;
using DentAssist.Models;

namespace DentAssist.Web.Controllers
{
    // Controlador para la gestión de tratamientos, accesible solo por Administrador o Recepcionista
    [Authorize(Roles = "Administrador,Recepcionista")]
    public class TratamientosController : Controller
    {
        // Contexto de base de datos para acceder a los tratamientos
        private readonly DentAssistContext _context;

        // Inyección de contexto por constructor
        public TratamientosController(DentAssistContext context)
        {
            _context = context;
        }

        // ========================================================
        // LISTADO DE TRATAMIENTOS
        // ========================================================
        public IActionResult Index()
        {
            // Carga todos los tratamientos desde la base de datos
            List<Tratamiento> lista = new List<Tratamiento>();
            foreach (Tratamiento t in _context.Tratamientos)
            {
                lista.Add(t);
            }
            return View(lista);
        }

        // ========================================================
        // CREAR NUEVO TRATAMIENTO (GET y POST)
        // ========================================================
        // Muestra el formulario de creación
        public IActionResult Create()
        {
            ViewData["Title"] = "Crear Tratamiento";
            return View(new Tratamiento());
        }

        // Procesa el alta del tratamiento
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Tratamiento model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Crear Tratamiento";
                return View(model);
            }

            _context.Tratamientos.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // EDITAR TRATAMIENTO (GET y POST)
        // ========================================================
        // Muestra el formulario de edición
        public IActionResult Edit(int id)
        {
            var tr = _context.Tratamientos.Find(id);
            if (tr == null) return NotFound();
            ViewData["Title"] = "Editar Tratamiento";
            return View(tr);
        }

        // Procesa la edición y guarda los cambios
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tratamiento model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Editar Tratamiento";
                return View(model);
            }

            var tr = _context.Tratamientos.Find(id);
            if (tr == null) return NotFound();
            tr.Nombre = model.Nombre;
            tr.Descripcion = model.Descripcion;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // ELIMINAR TRATAMIENTO (GET y POST)
        // ========================================================
        // Muestra confirmación de eliminación
        public IActionResult Delete(int id)
        {
            var tr = _context.Tratamientos.Find(id);
            if (tr == null) return NotFound();
            ViewData["Title"] = "Eliminar Tratamiento";
            return View(tr);
        }

        // Confirma y ejecuta la eliminación
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tr = _context.Tratamientos.Find(id);
            if (tr != null)
            {
                _context.Tratamientos.Remove(tr);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
