using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class AgregarCamposIndicador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Objetivo",
                table: "Indicador",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Medida",
                table: "Indicador",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Fecha_Aproximada",
                table: "Indicador",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Periodo",
                table: "Indicador",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableId",
                table: "Indicador",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Indicador_ResponsableId",
                table: "Indicador",
                column: "ResponsableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indicador_AspNetUsers_ResponsableId",
                table: "Indicador",
                column: "ResponsableId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indicador_AspNetUsers_ResponsableId",
                table: "Indicador");

            migrationBuilder.DropIndex(
                name: "IX_Indicador_ResponsableId",
                table: "Indicador");

            migrationBuilder.DropColumn(
                name: "Fecha_Aproximada",
                table: "Indicador");

            migrationBuilder.DropColumn(
                name: "Periodo",
                table: "Indicador");

            migrationBuilder.DropColumn(
                name: "ResponsableId",
                table: "Indicador");

            migrationBuilder.AlterColumn<double>(
                name: "Objetivo",
                table: "Indicador",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Medida",
                table: "Indicador",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
