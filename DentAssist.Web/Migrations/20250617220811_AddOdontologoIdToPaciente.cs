using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddOdontologoIdToPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OdontologoId",
                table: "Pacientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_OdontologoId",
                table: "Pacientes",
                column: "OdontologoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Odontologo_OdontologoId",
                table: "Pacientes",
                column: "OdontologoId",
                principalTable: "Odontologo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Odontologo_OdontologoId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_OdontologoId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "OdontologoId",
                table: "Pacientes");
        }
    }
}
