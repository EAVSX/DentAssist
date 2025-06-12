using System;

namespace DentAssist.Web.Models
{
    public class PacienteViewModel
    {
        private int _id;
        private string _nombre;
        private string _apellido;
        private DateTime _fechaNacimiento;
        private string _telefono;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }

        public DateTime FechaNacimiento
        {
            get { return _fechaNacimiento; }
            set { _fechaNacimiento = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }
    }
}
