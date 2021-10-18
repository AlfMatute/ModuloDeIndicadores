using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuloDeIndicadores.Data.Migrations
{
    public partial class NotasAdjunto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Adjunto",
                table: "Nota",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adjunto",
                table: "Nota");
        }
    }
}
