using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiPeliculas.Migrations
{
    public partial class CambioCampoUsuarioTablaUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UasuarioA",
                table: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioA",
                table: "Usuario",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioA",
                table: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "UasuarioA",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
