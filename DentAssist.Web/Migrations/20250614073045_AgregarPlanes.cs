using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPlanes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanTratamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTratamientos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlanTratamientoPasos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlanTratamientoId = table.Column<int>(type: "int", nullable: false),
                    TratamientoId = table.Column<int>(type: "int", nullable: false),
                    Secuencia = table.Column<int>(type: "int", nullable: false),
                    FechaEstimada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTratamientoPasos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTratamientoPasos_PlanTratamientos_PlanTratamientoId",
                        column: x => x.PlanTratamientoId,
                        principalTable: "PlanTratamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanTratamientoPasos_Tratamientos_TratamientoId",
                        column: x => x.TratamientoId,
                        principalTable: "Tratamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientoPasos_PlanTratamientoId",
                table: "PlanTratamientoPasos",
                column: "PlanTratamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientoPasos_TratamientoId",
                table: "PlanTratamientoPasos",
                column: "TratamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientos_PacienteId",
                table: "PlanTratamientos",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanTratamientoPasos");

            migrationBuilder.DropTable(
                name: "PlanTratamientos");
        }
    }
}
