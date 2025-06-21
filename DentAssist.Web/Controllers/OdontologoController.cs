using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Web.Datos;
using DentAssist.Web.Models;

namespace DentAssist.Web.Controllers
{
    // Controlador solo accesible por administradores, gestiona CRUD de odontólogos y usuarios asociados
    [Authorize(Roles = "Administrador")]
    public class OdontologoController : Controller
    {
        // Dependencias para acceso a base de datos, usuarios y roles
        private readonly DentAssistContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Inyección de dependencias vía constructor
        public OdontologoController(
            DentAssistContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ========================================================
        // LISTADO GENERAL DE ODONTÓLOGOS
        // ========================================================
        // Muestra todos los odontólogos registrados en el sistema
        public IActionResult Index()
        {
            List<Odontologo> lista = new List<Odontologo>();
            foreach (Odontologo o in _context.Odontologo)
            {
                lista.Add(o);
            }
            return View(lista);
        }

        // ========================================================
        // CREAR NUEVO ODONTÓLOGO (GET Y POST)
        // ========================================================
        // Formulario de registro para nuevo odontólogo
        public IActionResult Create()
        {
            return View();
        }

        // Procesa el alta de un odontólogo y su usuario Identity
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OdontologoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Guarda odontólogo en la base relacional
            Odontologo odo = new Odontologo
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Rut = model.Rut,
                Especialidad = model.Especialidad,
                Direccion = model.Direccion,
                Telefono = model.Telefono,
                Email = model.Email
            };
            _context.Odontologo.Add(odo);
            _context.SaveChanges();

            // Crea el usuario en Identity con el mismo email y contraseña
            IdentityUser user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            IdentityResult creacion = await _userManager.CreateAsync(user, model.Password);
            if (!creacion.Succeeded)
            {
                // Si falla la creación del usuario, elimina el odontólogo (rollback manual)
                foreach (var err in creacion.Errors)
                    ModelState.AddModelError("", err.Description);

                _context.Odontologo.Remove(odo);
                _context.SaveChanges();
                return View(model);
            }

            // Asegura que exista el rol "Odontólogo" y lo asigna al nuevo usuario
            bool existeRol = await _roleManager.RoleExistsAsync("Odontólogo");
            if (!existeRol)
                await _roleManager.CreateAsync(new IdentityRole("Odontólogo"));
            await _userManager.AddToRoleAsync(user, "Odontólogo");

            return RedirectToAction("Index");
        }

        // ========================================================
        // EDITAR ODONTÓLOGO (GET Y POST)
        // ========================================================
        // Muestra el formulario con los datos actuales del odontólogo
        public IActionResult Edit(int id)
        {
            Odontologo odo = _context.Odontologo.Find(id);
            if (odo == null) return NotFound();

            OdontologoViewModel vm = new OdontologoViewModel
            {
                Nombre = odo.Nombre,
                Apellido = odo.Apellido,
                Rut = odo.Rut,
                Especialidad = odo.Especialidad,
                Direccion = odo.Direccion,
                Telefono = odo.Telefono,
                Email = odo.Email
            };
            return View(vm);
        }

        // Procesa la edición y guarda los cambios
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OdontologoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Odontologo odo = _context.Odontologo.Find(id);
            if (odo == null) return NotFound();

            odo.Nombre = model.Nombre;
            odo.Apellido = model.Apellido;
            odo.Rut = model.Rut;
            odo.Especialidad = model.Especialidad;
            odo.Direccion = model.Direccion;
            odo.Telefono = model.Telefono;
            odo.Email = model.Email;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ========================================================
        // ELIMINAR ODONTÓLOGO (GET Y POST)
        // ========================================================
        // Muestra confirmación antes de eliminar
        public IActionResult Delete(int id)
        {
            Odontologo odo = _context.Odontologo.Find(id);
            if (odo == null) return NotFound();
            return View(odo);
        }

        // Confirma y ejecuta la eliminación del odontólogo
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Odontologo odo = _context.Odontologo.Find(id);
            if (odo != null)
            {
                _context.Odontologo.Remove(odo);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
