using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    public class PasoTratamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // FK al plan padre (lo mantienes, pero sin Required)
        public int PlanTratamientoId { get; set; }

        [ForeignKey(nameof(PlanTratamientoId))]
        public PlanTratamiento PlanTratamiento { get; set; }

        // FK al Tratamiento (sin Required)
        public int TratamientoId { get; set; }

        [ForeignKey(nameof(TratamientoId))]
        public Tratamiento Tratamiento { get; set; }

        // Descripción será asignada desde el Tratamiento.Nombre
        [StringLength(200)]
        public string Descripcion { get; set; }

        // Fecha estimada (sin Required)
        public DateTime FechaEstimada { get; set; }

        // Estado (sin Required)
        [StringLength(20)]
        public string Estado { get; set; }

        // Precio que también llenas desde el Tratamiento.Precio
        public decimal Precio { get; set; }

        [StringLength(500)]
        public string ObservacionesClinicas { get; set; }
    }
}
