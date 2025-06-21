using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Models
{
    public class Paciente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [NotMapped]
        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }

        [Required, StringLength(20)]
        public string Rut { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        // FK al odontólogo
        [Required]
        public int OdontologoId { get; set; }

        [ForeignKey("OdontologoId")]
        public Odontologo Odontologo { get; set; }

        // Relaciones
        public ICollection<Turno> Turnos { get; set; }
        public ICollection<PlanTratamiento> Planes { get; set; }
    }
}
