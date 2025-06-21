using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    public class Configuracion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la clínica es obligatorio")]
        [StringLength(150)]
        public string NombreClinica { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(30)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(80)]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string EmailContacto { get; set; }
    }
}
