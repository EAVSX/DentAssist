using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    // Modelo que representa cada uno de los pasos individuales dentro de un plan de tratamiento dental.
    // Permite detallar el avance, estado y particularidades de cada etapa específica de un tratamiento.
    // Relaciona cada paso con su plan padre y el tratamiento base correspondiente (mediante claves foráneas).
    // Incluye campos para descripción, fecha estimada, estado, precio (heredado del tratamiento) y observaciones clínicas.
    // Facilita el seguimiento granular del avance clínico y administrativo de cada paciente.
    public class PasoTratamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PlanTratamientoId { get; set; }

        [ForeignKey(nameof(PlanTratamientoId))]
        public PlanTratamiento PlanTratamiento { get; set; }

        public int TratamientoId { get; set; }

        [ForeignKey(nameof(TratamientoId))]
        public Tratamiento Tratamiento { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public DateTime FechaEstimada { get; set; }

        [StringLength(20)]
        public string Estado { get; set; }

        public decimal Precio { get; set; }

        [StringLength(500)]
        public string ObservacionesClinicas { get; set; }
    }
}
