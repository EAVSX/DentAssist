using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class TurnoCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Turnos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Duracion",
                table: "Turnos",
                type: "int",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Turnos",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "PasoTratamientos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEstimada",
                table: "PasoTratamientos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesClinicas",
                table: "PasoTratamientos",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "PasoTratamientos");

            migrationBuilder.DropColumn(
                name: "FechaEstimada",
                table: "PasoTratamientos");

            migrationBuilder.DropColumn(
                name: "ObservacionesClinicas",
                table: "PasoTratamientos");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Turnos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duracion",
                table: "Turnos",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
