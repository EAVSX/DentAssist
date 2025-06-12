using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    public class PacientesController : Controller
    {
        // GET: /Pacientes
        public IActionResult Index()
        {
            // Creamos lista de prueba
            List<PacienteViewModel> listaPacientes = new List<PacienteViewModel>();

            // Paciente 1
            PacienteViewModel paciente1 = new PacienteViewModel();
            paciente1.Id = 1;
            paciente1.Nombre = "Juan";
            paciente1.Apellido = "Pérez";
            paciente1.FechaNacimiento = new DateTime(1985, 4, 10);
            paciente1.Telefono = "912345678";
            listaPacientes.Add(paciente1);

            // Paciente 2
            PacienteViewModel paciente2 = new PacienteViewModel();
            paciente2.Id = 2;
            paciente2.Nombre = "María";
            paciente2.Apellido = "González";
            paciente2.FechaNacimiento = new DateTime(1990, 9, 5);
            paciente2.Telefono = "987654321";
            listaPacientes.Add(paciente2);

            // Devolvemos la vista con la lista
            return View(listaPacientes);
        }

        // GET: /Pacientes/Create
        public IActionResult Create()
        {
            // Devuelve el formulario vacío
            return View();
        }

        // POST: /Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PacienteViewModel model)
        {
            // Validación en servidor
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // TODO: Lógica para guardar en base de datos
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar el paciente.");
                return View(model);
            }
        }
    }
}
