using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Models
{
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

        // Relaciones
        public ICollection<Paciente> Pacientes { get; set; }
        public ICollection<Turno> Turnos { get; set; }
    }
}
