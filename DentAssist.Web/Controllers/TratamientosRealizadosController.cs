using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;
using DentAssist.Models;

// Controlador para gestionar los tratamientos realizados a cada paciente.
// Permite listar, ver detalle, crear, editar y eliminar tratamientos realizados.

namespace DentAssist.Web.Controllers
{
    public class TratamientosRealizadosController : Controller
    {
        // Contexto para acceso a la base de datos
        private readonly DentAssistContext _context;

        // Inyección del contexto por constructor
        public TratamientosRealizadosController(DentAssistContext context)
        {
            _context = context;
        }

        // ========================================================
        // LISTADO DE TRATAMIENTOS REALIZADOS POR PACIENTE
        // ========================================================
        public IActionResult Index(int pacienteId)
        {
            // Guarda el ID de paciente para usar en la vista
            ViewData["PacienteId"] = pacienteId;

            // Lista todos los tratamientos realizados de ese paciente (con info de tratamiento)
            List<TratamientoRealizado> lista = new List<TratamientoRealizado>();
            foreach (TratamientoRealizado tr in _context.TratamientosRealizados)
            {
                if (tr.PacienteId == pacienteId)
                {
                    // Carga el tratamiento relacionado
                    tr.Tratamiento = _context.Tratamientos.Find(tr.TratamientoId);
                    lista.Add(tr);
                }
            }
            return View(lista);
        }

        // ========================================================
        // DETALLE DE UN TRATAMIENTO REALIZADO
        // ========================================================
        public IActionResult Details(int id)
        {
            // Busca el tratamiento realizado y sus relaciones
            TratamientoRealizado modelo = null;
            foreach (TratamientoRealizado tr in _context.TratamientosRealizados)
            {
                if (tr.Id == id)
                {
                    modelo = tr;
                    break;
                }
            }
            if (modelo == null) return NotFound();

            modelo.Tratamiento = _context.Tratamientos.Find(modelo.TratamientoId);
            modelo.Paciente = _context.Pacientes.Find(modelo.PacienteId);
            return View(modelo);
        }

        // ========================================================
        // CREAR NUEVO TRATAMIENTO REALIZADO (GET y POST)
        // ========================================================
        public IActionResult Create(int pacienteId)
        {
            ViewData["PacienteId"] = pacienteId;
            List<Tratamiento> lista = new List<Tratamiento>();
            foreach (Tratamiento t in _context.Tratamientos)
            {
                lista.Add(t);
            }
            ViewData["TratamientosList"] = new SelectList(lista, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TratamientoRealizado modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["PacienteId"] = modelo.PacienteId;
                List<Tratamiento> lista = new List<Tratamiento>();
                foreach (Tratamiento t in _context.Tratamientos)
                {
                    lista.Add(t);
                }
                ViewData["TratamientosList"] = new SelectList(lista, "Id", "Nombre", modelo.TratamientoId);
                return View(modelo);
            }

            _context.TratamientosRealizados.Add(modelo);
            _context.SaveChanges();
            return RedirectToAction("Index", new { pacienteId = modelo.PacienteId });
        }

        // ========================================================
        // EDITAR UN TRATAMIENTO REALIZADO (GET y POST)
        // ========================================================
        public IActionResult Edit(int id)
        {
            TratamientoRealizado modelo = _context.TratamientosRealizados.Find(id);
            if (modelo == null) return NotFound();

            ViewData["PacienteId"] = modelo.PacienteId;
            List<Tratamiento> lista = new List<Tratamiento>();
            foreach (Tratamiento t in _context.Tratamientos)
            {
                lista.Add(t);
            }
            ViewData["TratamientosList"] = new SelectList(lista, "Id", "Nombre", modelo.TratamientoId);
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TratamientoRealizado modelo)
        {
            if (id != modelo.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["PacienteId"] = modelo.PacienteId;
                List<Tratamiento> lista = new List<Tratamiento>();
                foreach (Tratamiento t in _context.Tratamientos)
                {
                    lista.Add(t);
                }
                ViewData["TratamientosList"] = new SelectList(lista, "Id", "Nombre", modelo.TratamientoId);
                return View(modelo);
            }

            TratamientoRealizado existente = _context.TratamientosRealizados.Find(id);
            if (existente == null) return NotFound();

            // Actualiza campos principales
            existente.FechaRealizacion = modelo.FechaRealizacion;
            existente.TratamientoId = modelo.TratamientoId;
            existente.Observaciones = modelo.Observaciones;
            _context.SaveChanges();

            return RedirectToAction("Index", new { pacienteId = existente.PacienteId });
        }

        // ========================================================
        // ELIMINAR TRATAMIENTO REALIZADO (GET y POST)
        // ========================================================
        public IActionResult Delete(int id)
        {
            TratamientoRealizado modelo = null;
            foreach (TratamientoRealizado tr in _context.TratamientosRealizados)
            {
                if (tr.Id == id)
                {
                    modelo = tr;
                    break;
                }
            }
            if (modelo == null) return NotFound();

            modelo.Tratamiento = _context.Tratamientos.Find(modelo.TratamientoId);
            return View(modelo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TratamientoRealizado modelo = _context.TratamientosRealizados.Find(id);
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
