using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models
{
    // ViewModel para el formulario de inicio de sesión (login) de usuarios.
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        // Opción para recordar la sesión del usuario (login persistente)
        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        [Display(Name = "Return URL")]
        public string? ReturnUrl { get; set; }
    }
}
