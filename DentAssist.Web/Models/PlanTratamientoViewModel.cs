using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models.ViewModels
{
    // ViewModel para el formulario de alta y edición de planes de tratamiento.
    // Solo expone los campos requeridos al momento de crear un plan: paciente, tratamiento y observaciones.
    // Incluye validaciones clásicas para asegurar que se seleccionen opciones válidas y que las observaciones sean obligatorias.
    // Facilita la separación entre la lógica de la base de datos y lo que realmente se requiere en el formulario de la vista.
    public class PlanTratamientoViewModel
    {
        [Display(Name = "Paciente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un paciente.")]
        public int PacienteId { get; set; }

        [Display(Name = "Tratamiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tratamiento.")]
        public int TratamientoId { get; set; }

        [Display(Name = "Observaciones")]
        [Required(ErrorMessage = "Las observaciones son obligatorias.")]
        [StringLength(500)]
        public string Observaciones { get; set; }
    }
}
