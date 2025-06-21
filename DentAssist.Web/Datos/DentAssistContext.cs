using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DentAssist.Web.Models;
using DentAssist.Models;

// Contexto principal de la base de datos para DentAssist
// Hereda de IdentityDbContext para integrar autenticación y gestión de usuarios
namespace DentAssist.Web.Datos
{
    public class DentAssistContext : IdentityDbContext
    {
        // Constructor: recibe opciones de configuración (cadena de conexión, proveedor, etc.)
        public DentAssistContext(DbContextOptions<DentAssistContext> options)
            : base(options)
        {
        }

        // DbSet para cada entidad/tablas de la base de datos
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Odontologo> Odontologo { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<TratamientoRealizado> TratamientosRealizados { get; set; }
        public DbSet<PlanTratamiento> PlanTratamientos { get; set; }
        public DbSet<PasoTratamiento> PasoTratamientos { get; set; }

        // Configuración adicional de modelos (tablas) al crear la base de datos
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Incluye la configuración estándar de Identity
            base.OnModelCreating(builder);

            // Se marca PlanTratamiento.Id y PasoTratamiento.Id como auto-incrementales
            builder.Entity<PlanTratamiento>()
                   .Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Entity<PasoTratamiento>()
                   .Property(p => p.Id)
                   .ValueGeneratedOnAdd();
        }
    }
}
