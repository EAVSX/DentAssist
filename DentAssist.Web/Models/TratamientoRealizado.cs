using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    /// <summary>
    /// Registra un tratamiento ejecutado en el historial clínico de un paciente.
    /// </summary>
    public class TratamientoRealizado
    {
        private int _id;
        private DateTime _fechaRealizacion;
        private int _pacienteId;
        private Paciente _paciente;
        private int _tratamientoId;
        private Tratamiento _tratamiento;
        private string _observaciones;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required(ErrorMessage = "La fecha de realización es obligatoria")]
        [Display(Name = "Fecha de Realización")]
        public DateTime FechaRealizacion
        {
            get { return _fechaRealizacion; }
            set { _fechaRealizacion = value; }
        }

        [ForeignKey("Paciente")]
        [Display(Name = "Paciente")]
        public int PacienteId
        {
            get { return _pacienteId; }
            set { _pacienteId = value; }
        }

        /// <summary>
        /// Paciente al que se le realizó el tratamiento.
        /// </summary>
        public Paciente Paciente
        {
            get { return _paciente; }
            set { _paciente = value; }
        }

        [ForeignKey("Tratamiento")]
        [Display(Name = "Tratamiento")]
        public int TratamientoId
        {
            get { return _tratamientoId; }
            set { _tratamientoId = value; }
        }

        /// <summary>
        /// Detalle del tratamiento aplicado.
        /// </summary>
        public Tratamiento Tratamiento
        {
            get { return _tratamiento; }
            set { _tratamiento = value; }
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string Observaciones
        {
            get { return _observaciones; }
            set { _observaciones = value; }
        }
    }
}
