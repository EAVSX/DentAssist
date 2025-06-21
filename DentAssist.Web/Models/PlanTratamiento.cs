using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    // Modelo que representa un plan de tratamiento dental completo para un paciente.
    // Incluye la relación directa con el paciente y el tratamiento base asociado, así como el precio y la fecha de creación.
    // Permite agregar observaciones y agrupa todos los pasos específicos que componen el plan.
    // Facilita la trazabilidad clínica y administrativa del proceso terapéutico, permitiendo consultar historial y detalles.
    public class PlanTratamiento
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Clave foránea al paciente que recibe el plan
        [Required]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente Paciente { get; set; }

        [Required]
        public int TratamientoId { get; set; }

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
