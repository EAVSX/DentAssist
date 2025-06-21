using System.Collections.Generic;

namespace DentAssist.Web.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<RoleItem> Roles { get; set; }
    }

    public class RoleItem
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
