using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models
{
    // ViewModel para el formulario de alta o edición de recepcionistas en la aplicación web.
    // Incluye campos con validaciones clásicas para nombre, email y contraseña (con confirmación).
    // Permite separar la lógica de presentación de la entidad principal, facilitando el uso en formularios y vistas.
    public class RecepcionistaViewModel
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
