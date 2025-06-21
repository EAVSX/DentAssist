using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación es obligatoria")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
