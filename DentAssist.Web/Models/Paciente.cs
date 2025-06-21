using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Models
{
    // Modelo que representa a los pacientes de la clínica.
    // Incluye los datos principales de identificación y contacto, además de la relación con el odontólogo responsable.
    // Dispone de una propiedad calculada (NombreCompleto) para uso en vistas y listados.
    // Establece relaciones clásicas: un paciente tiene muchos turnos y varios planes de tratamiento.
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

        // Clave foránea: odontólogo responsable del paciente
        [Required]
        public int OdontologoId { get; set; }

        [ForeignKey("OdontologoId")]
        public Odontologo Odontologo { get; set; }

        // Relaciones: turnos y planes de tratamiento del paciente
        public ICollection<Turno> Turnos { get; set; }
        public ICollection<PlanTratamiento> Planes { get; set; }
    }
}
