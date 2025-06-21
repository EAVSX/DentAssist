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
    [Authorize(Roles = "Administrador")]
    public class OdontologoController : Controller
    {
        private readonly DentAssistContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public OdontologoController(
            DentAssistContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // --------------------------------------------------
        // LISTAR ODONTÓLOGOS
        // GET: /Odontologo
        // --------------------------------------------------
        public IActionResult Index()
        {
            List<Odontologo> lista = new List<Odontologo>();
            foreach (Odontologo o in _context.Odontologo)
            {
                lista.Add(o);
            }
            return View(lista);
        }

        // --------------------------------------------------
        // CREAR ODONTÓLOGO
        // GET: /Odontologo/Create
        // --------------------------------------------------
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Odontologo/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OdontologoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) Guardar el Odontólogo en EF
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

            // 2) Crear el usuario en Identity
            IdentityUser user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            IdentityResult creacion = await _userManager.CreateAsync(user, model.Password);
            if (!creacion.Succeeded)
            {
                // Si falla el IdentityUser, mostrar errores y eliminar el Odontólogo
                foreach (var err in creacion.Errors)
                    ModelState.AddModelError("", err.Description);

                // Rollback manual
                _context.Odontologo    .Remove(odo);
                _context.SaveChanges();

                return View(model);
            }

            // 3) Asegurarse de que exista el rol "Odontólogo"
            bool existeRol = await _roleManager.RoleExistsAsync("Odontólogo");
            if (!existeRol)
                await _roleManager.CreateAsync(new IdentityRole("Odontólogo"));

            // 4) Asignar al nuevo usuario el rol
            await _userManager.AddToRoleAsync(user, "Odontólogo");

            return RedirectToAction("Index");
        }

        // --------------------------------------------------
        // EDITAR ODONTÓLOGO
        // GET: /Odontologo/Edit/5
        // --------------------------------------------------
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

        // POST: /Odontologo/Edit/5
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

        // --------------------------------------------------
        // ELIMINAR ODONTÓLOGO
        // GET: /Odontologo/Delete/5
        // --------------------------------------------------
        public IActionResult Delete(int id)
        {
            Odontologo odo = _context.Odontologo.Find(id);
            if (odo == null) return NotFound();
            return View(odo);
        }

        // POST: /Odontologo/Delete/5
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
