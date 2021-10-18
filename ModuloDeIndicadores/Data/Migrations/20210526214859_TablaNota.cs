using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class TablaNota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    Id_Nota = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndicadorId_Indicador = table.Column<int>(nullable: true),
                    Logro = table.Column<double>(nullable: false),
                    Puntos = table.Column<double>(nullable: false),
                    Mes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.Id_Nota);
                    table.ForeignKey(
                        name: "FK_Nota_Indicador_IndicadorId_Indicador",
                        column: x => x.IndicadorId_Indicador,
                        principalTable: "Indicador",
                        principalColumn: "Id_Indicador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IndicadorId_Indicador",
                table: "Nota",
                column: "IndicadorId_Indicador");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nota");
        }
    }
}
