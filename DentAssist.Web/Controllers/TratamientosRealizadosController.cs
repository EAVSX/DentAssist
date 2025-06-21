using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;
using System.Linq;

namespace DentAssist.Web.Controllers
{
    public class TratamientosRealizadosController : Controller
    {
        private readonly DentAssistContext _context;

        public TratamientosRealizadosController(DentAssistContext context)
        {
            _context = context;
        }

        // GET: /TratamientosRealizados?pacienteId=5
        public IActionResult Index(int pacienteId)
        {
            ViewData["PacienteId"] = pacienteId;
            var lista = _context.TratamientosRealizados
                                .Include(tr => tr.Tratamiento)
                                .Where(tr => tr.PacienteId == pacienteId)
                                .ToList();
            return View(lista);
        }

        // GET: /TratamientosRealizados/Details/5
        public IActionResult Details(int id)
        {
            var modelo = _context.TratamientosRealizados
                                 .Include(tr => tr.Tratamiento)
                                 .Include(tr => tr.Paciente)
                                 .FirstOrDefault(tr => tr.Id == id);
            if (modelo == null) return NotFound();
            return View(modelo);
        }

        // GET: /TratamientosRealizados/Create?pacienteId=5
        public IActionResult Create(int pacienteId)
        {
            ViewData["PacienteId"] = pacienteId;
            ViewData["TratamientosList"] = new SelectList(
                _context.Tratamientos.ToList(), "Id", "Nombre"
            );
            return View();
        }

        // POST: /TratamientosRealizados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TratamientoRealizado modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["PacienteId"] = modelo.PacienteId;
                ViewData["TratamientosList"] = new SelectList(
                    _context.Tratamientos.ToList(), "Id", "Nombre", modelo.TratamientoId
                );
                return View(modelo);
            }

            _context.TratamientosRealizados.Add(modelo);
            _context.SaveChanges();
            return RedirectToAction("Index", new { pacienteId = modelo.PacienteId });
        }

        // GET: /TratamientosRealizados/Edit/5
        public IActionResult Edit(int id)
        {
            var modelo = _context.TratamientosRealizados.Find(id);
            if (modelo == null) return NotFound();

            ViewData["PacienteId"] = modelo.PacienteId;
            ViewData["TratamientosList"] = new SelectList(
                _context.Tratamientos.ToList(), "Id", "Nombre", modelo.TratamientoId
            );
            return View(modelo);
        }

        // POST: /TratamientosRealizados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TratamientoRealizado modelo)
        {
            if (id != modelo.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["PacienteId"] = modelo.PacienteId;
                ViewData["TratamientosList"] = new SelectList(
                    _context.Tratamientos.ToList(), "Id", "Nombre", modelo.TratamientoId
                );
                return View(modelo);
            }

            var existente = _context.TratamientosRealizados.Find(id);
            if (existente == null) return NotFound();

            existente.FechaRealizacion = modelo.FechaRealizacion;
            existente.TratamientoId = modelo.TratamientoId;
            existente.Observaciones = modelo.Observaciones;
            _context.SaveChanges();

            return RedirectToAction("Index", new { pacienteId = existente.PacienteId });
        }

        // GET: /TratamientosRealizados/Delete/5
        public IActionResult Delete(int id)
        {
            var modelo = _context.TratamientosRealizados
                                 .Include(tr => tr.Tratamiento)
                                 .FirstOrDefault(tr => tr.Id == id);
            if (modelo == null) return NotFound();
            return View(modelo);
        }

        // POST: /TratamientosRealizados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var modelo = _context.TratamientosRealizados.Find(id);
            if (modelo != null)
            {
                int pid = modelo.PacienteId;
                _context.TratamientosRealizados.Remove(modelo);
                _context.SaveChanges();
                return RedirectToAction("Index", new { pacienteId = pid });
            }
            return NotFound();
        }
    }
}
