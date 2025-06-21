using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Models
{
    // Modelo que representa a los odontólogos registrados en el sistema.
    // Incluye información básica de identificación, contacto, especialidad y rut (nacional).
    public class Odontologo
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

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Rut { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(100)]
        public string Especialidad { get; set; }

        // Relaciones: un odontólogo puede tener varios pacientes y varios turnos asignados
        public ICollection<Paciente> Pacientes { get; set; }
        public ICollection<Turno> Turnos { get; set; }
    }
}
