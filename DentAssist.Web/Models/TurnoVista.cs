// Models/TurnoVista.cs
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
