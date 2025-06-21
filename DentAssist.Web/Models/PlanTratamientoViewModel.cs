// Models/ViewModels/PlanTratamientoViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Web.Models.ViewModels
{
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
