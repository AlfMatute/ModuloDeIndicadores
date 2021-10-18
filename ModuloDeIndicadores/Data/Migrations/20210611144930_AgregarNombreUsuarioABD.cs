using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class AgregarNombreUsuarioABD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "AspNetUsers");
        }
    }
}
