// Models/PlanTratamiento.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    public class PlanTratamiento
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente Paciente { get; set; }

        [Required]
        public int TratamientoId { get; set; }      // <-- Nueva FK

        [ForeignKey(nameof(TratamientoId))]
        public Tratamiento Tratamiento { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required, StringLength(500)]
        public string Observaciones { get; set; }

        public ICollection<PasoTratamiento> Pasos { get; set; } = new List<PasoTratamiento>();
    }
}
