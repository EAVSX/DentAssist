using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregaTratamientoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TratamientoId",
                table: "PlanTratamientos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlanTratamientos_TratamientoId",
                table: "PlanTratamientos",
                column: "TratamientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTratamientos_Tratamientos_TratamientoId",
                table: "PlanTratamientos",
                column: "TratamientoId",
                principalTable: "Tratamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanTratamientos_Tratamientos_TratamientoId",
                table: "PlanTratamientos");

            migrationBuilder.DropIndex(
                name: "IX_PlanTratamientos_TratamientoId",
                table: "PlanTratamientos");

            migrationBuilder.DropColumn(
                name: "TratamientoId",
                table: "PlanTratamientos");
        }
    }
}
