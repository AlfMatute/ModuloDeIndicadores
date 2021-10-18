using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class AgregarDescripcionANota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Nota",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Nota");
        }
    }
}
