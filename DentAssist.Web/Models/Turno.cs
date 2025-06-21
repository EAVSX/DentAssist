using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Models
{
    // Modelo que representa un turno o cita en la clínica dental.
    // Incluye paciente, odontólogo, fecha, duración, estado y observaciones.
    public class Turno
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un odontólogo.")]
        public int OdontologoId { get; set; }

        [ForeignKey("OdontologoId")]
        public virtual Odontologo Odontologo { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(10, 240, ErrorMessage = "La duración debe ser entre 10 y 240 minutos.")]
        public int Duracion { get; set; }

        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public EstadoTurno Estado { get; set; } = EstadoTurno.Programado; // Valor por defecto
    }

    // Enum para los posibles estados de un turno
    public enum EstadoTurno
    {
        Programado = 0,
        Confirmado = 1,
        Atendido = 2,
        Cancelado = 3,
        Finalizado = 4
    }
}
