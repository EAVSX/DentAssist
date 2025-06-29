﻿using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    // Modelo para representar a los recepcionistas registrados en la clínica.
    // Incluye los campos esenciales para la gestión y autenticación: nombre, email y contraseña.
    // Se aplican validaciones clásicas para asegurar datos completos y en formato correcto.
    public class Recepcionista
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }
    }
}
