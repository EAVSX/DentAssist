using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models
{
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

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        // Ahora es nullable para no ser obligatorio
        [Display(Name = "Return URL")]
        public string? ReturnUrl { get; set; }
    }
}
