using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    // Modelo que representa los tratamientos dentales disponibles en la clínica.
    // Incluye los campos básicos para identificación, descripción y precio de cada tratamiento.
    // Aplica validaciones clásicas para asegurar datos completos y en formato correcto.
    public class Tratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(250)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 9999999, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Precio { get; set; }
    }
}
