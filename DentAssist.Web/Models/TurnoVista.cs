// Modelo de solo lectura para mostrar turnos en la interfaz.
// Incluye datos listos para mostrar: paciente, odontólogo, fecha, duración, estado y observaciones.
namespace DentAssist.Web.Models
{
    public class TurnoVista
    {
        public int Id { get; set; }
        public System.DateTime FechaHora { get; set; }
        public int Duracion { get; set; }
        public string PacienteNombre { get; set; }
        public string OdontologoNombre { get; set; }
        public EstadoTurno Estado { get; set; }
        public string Observaciones { get; set; }
    }
}
