using System.Collections.Generic;

namespace DentAssist.Web.Models
{
    // ViewModel para editar los roles de un usuario en la interfaz de administración
    public class EditRolesViewModel
    {
        
        public string UserId { get; set; }
       
        public string Email { get; set; }
        
        public List<RoleItem> Roles { get; set; }
    }

    // Modelo auxiliar para representar cada rol y si está asignado al usuario
    public class RoleItem
    {
        
        public string RoleName { get; set; }
        
        public bool IsSelected { get; set; }
    }
}
