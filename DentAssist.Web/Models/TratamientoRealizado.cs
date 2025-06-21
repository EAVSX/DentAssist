using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentAssist.Models;

namespace DentAssist.Web.Models
{
    /// Modelo que registra cada tratamiento realizado en el historial clínico de un paciente.
    /// Incluye la fecha de realización, paciente, tratamiento aplicado y observaciones adicionales.
    /// Se asocia por claves foráneas tanto al paciente como al tratamiento, permitiendo navegación clásica.
    /// Facilita la trazabilidad y auditoría de procedimientos efectuados a cada paciente en la clínica.
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
        
        /// Paciente al que se le realizó el tratamiento.
     
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

        /// Detalle del tratamiento aplicado.
     
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
