using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DentAssist.Web.Models;
using DentAssist.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Web.Datos
{
    public class DentAssistContext : IdentityDbContext
    {
        public DentAssistContext(DbContextOptions<DentAssistContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Odontologo> Odontologo { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }

        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<TratamientoRealizado> TratamientosRealizados { get; set; }

        public DbSet<PlanTratamiento> PlanTratamientos { get; set; }
        public DbSet<PasoTratamiento> PasoTratamientos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Marca PlanTratamiento.Id como auto-incremental
            builder.Entity<PlanTratamiento>()
                   .Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            // Marca PasoTratamiento.Id como auto-incremental
            builder.Entity<PasoTratamiento>()
                   .Property(p => p.Id)
                   .ValueGeneratedOnAdd();
        }
    }
}
