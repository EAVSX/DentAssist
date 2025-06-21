using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models
{
    // ViewModel usado para la creación y edición de odontólogos en formularios de la web.
    // Incluye todos los campos necesarios para registrar un odontólogo y validaciones de formato.
    // Se incluyen campos de contraseña y confirmación para gestionar el acceso de usuario asociado.
    public class OdontologoViewModel
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; }
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Apellido { get; set; }

        [Required, StringLength(12)]
        public string Rut { get; set; }

        [Required, StringLength(100)]
        public string Especialidad { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
