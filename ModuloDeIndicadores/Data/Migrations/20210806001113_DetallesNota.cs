using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class DetallesNota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdjuntoNombre",
                table: "Nota",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdjuntoTipo",
                table: "Nota",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjuntoNombre",
                table: "Nota");

            migrationBuilder.DropColumn(
                name: "AdjuntoTipo",
                table: "Nota");
        }
    }
}
