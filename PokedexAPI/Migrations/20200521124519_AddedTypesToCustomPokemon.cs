using Microsoft.EntityFrameworkCore.Migrations;

namespace PokedexAPI.Migrations
{
    public partial class AddedTypesToCustomPokemon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type1",
                table: "CustomPokemons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type2",
                table: "CustomPokemons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type1",
                table: "CustomPokemons");

            migrationBuilder.DropColumn(
                name: "Type2",
                table: "CustomPokemons");
        }
    }
}
