using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class QuitaSecuenciaYConfiguraPrecio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secuencia",
                table: "PasoTratamientos");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "PlanTratamientos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "PasoTratamientos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "PlanTratamientos");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "PasoTratamientos");

            migrationBuilder.AddColumn<int>(
                name: "Secuencia",
                table: "PasoTratamientos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
