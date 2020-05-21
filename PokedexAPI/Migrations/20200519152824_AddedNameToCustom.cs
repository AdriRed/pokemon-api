using Microsoft.EntityFrameworkCore.Migrations;

namespace PokedexAPI.Migrations
{
    public partial class AddedNameToCustom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomPokemons_Users_OwnerId",
                table: "CustomPokemons");

            migrationBuilder.DropIndex(
                name: "IX_CustomPokemons_OwnerId",
                table: "CustomPokemons");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CustomPokemons");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomPokemons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomPokemons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomPokemons_UserId",
                table: "CustomPokemons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPokemons_Users_UserId",
                table: "CustomPokemons",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomPokemons_Users_UserId",
                table: "CustomPokemons");

            migrationBuilder.DropIndex(
                name: "IX_CustomPokemons_UserId",
                table: "CustomPokemons");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomPokemons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomPokemons");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "CustomPokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomPokemons_OwnerId",
                table: "CustomPokemons",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPokemons_Users_OwnerId",
                table: "CustomPokemons",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
