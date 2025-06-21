using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentAssist.Web.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaTratamientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecioEstimado",
                table: "Tratamientos",
                newName: "Precio");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tratamientos",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Tratamientos",
                newName: "PrecioEstimado");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tratamientos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
