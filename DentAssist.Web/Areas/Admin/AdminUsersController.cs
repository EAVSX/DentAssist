using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DentAssist.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DentAssist.Web.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class AdminUsersController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        
        public AdminUsersController(UserManager<IdentityUser> userMgr,
                                    RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        // ===============================
        // Muestra el listado de usuarios
        // ===============================
        public IActionResult Index()
        {
            
            var users = _userMgr.Users;
            return View(users);
        }

        // ========================================================
        // Muestra el formulario para editar roles de un usuario
        // ========================================================
        public async Task<IActionResult> EditRoles(string id)
        {
            
            var user = await _userMgr.FindByIdAsync(id);
            if (user == null) return NotFound();


            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = new List<RoleItem>()
            };

            
            foreach (var role in _roleMgr.Roles)
            {
                model.Roles.Add(new RoleItem
                {
                    RoleName = role.Name,
                    IsSelected = await _userMgr.IsInRoleAsync(user, role.Name)
                });
            }

            return View(model);
        }

        // ========================================================
        // Guarda los cambios en los roles del usuario (POST)
        // ========================================================
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel vm)
        {
            
            var user = await _userMgr.FindByIdAsync(vm.UserId);
            if (user == null) return NotFound();

            
            var currentRoles = await _userMgr.GetRolesAsync(user);
            await _userMgr.RemoveFromRolesAsync(user, currentRoles);

           
            foreach (var role in vm.Roles)
            {
                if (role.IsSelected)
                    await _userMgr.AddToRoleAsync(user, role.RoleName);
            }

            
            return RedirectToAction("Index");
        }
    }
}
