using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;
using DentAssist.Models;

namespace DentAssist.Web.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista")]
    public class TratamientosController : Controller
    {
        private readonly DentAssistContext _context;
        public TratamientosController(DentAssistContext context)
        {
            _context = context;
        }

        // GET: /Tratamientos
        public IActionResult Index()
        {
            // Cargo todos los tratamientos desde el DbContext
            List<Tratamiento> lista = _context.Tratamientos.ToList();
            return View(lista);
        }

        // GET: /Tratamientos/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Crear Tratamiento";
            return View(new Tratamiento());
        }

        // POST: /Tratamientos/Create
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

        // GET: /Tratamientos/Edit/5
        public IActionResult Edit(int id)
        {
            var tr = _context.Tratamientos.Find(id);
            if (tr == null) return NotFound();
            ViewData["Title"] = "Editar Tratamiento";
            return View(tr);
        }

        // POST: /Tratamientos/Edit/5
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

        // GET: /Tratamientos/Delete/5
        public IActionResult Delete(int id)
        {
            var tr = _context.Tratamientos.Find(id);
            if (tr == null) return NotFound();
            ViewData["Title"] = "Eliminar Tratamiento";
            return View(tr);
        }

        // POST: /Tratamientos/Delete/5
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
