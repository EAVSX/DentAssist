using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPasswordOdontologo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanTratamientoPasos");

            migrationBuilder.AlterColumn<string>(
                name: "Matricula",
                table: "Odontologos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Odontologos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Odontologos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Odontologos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PasoTratamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlanTratamientoId = table.Column<int>(type: "int", nullable: false),
                    Secuencia = table.Column<int>(type: "int", nullable: false),
                    TratamientoId = table.Column<int>(type: "int", nullable: false),
                    FechaEstimada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasoTratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasoTratamientos_PlanTratamientos_PlanTratamientoId",
                        column: x => x.PlanTratamientoId,
                        principalTable: "PlanTratamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PasoTratamientos_Tratamientos_TratamientoId",
                        column: x => x.TratamientoId,
                        principalTable: "Tratamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PasoTratamientos_PlanTratamientoId",
                table: "PasoTratamientos",
                column: "PlanTratamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_PasoTratamientos_TratamientoId",
                table: "PasoTratamientos",
                column: "TratamientoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasoTratamientos");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Odontologos");

            migrationBuilder.AlterColumn<string>(
                name: "Matricula",
                table: "Odontologos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Odontologos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Odontologos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlanTratamientoPasos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlanTratamientoId = table.Column<int>(type: "int", nullable: false),
                    TratamientoId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaEstimada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observaciones = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Secuencia = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
