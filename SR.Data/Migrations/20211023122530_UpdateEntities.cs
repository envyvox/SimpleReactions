using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "discord_guilds");

            migrationBuilder.RenameColumn(
                name: "prefix",
                table: "discord_guilds",
                newName: "embed_color");

            migrationBuilder.RenameColumn(
                name: "language",
                table: "discord_guilds",
                newName: "language_type");

            migrationBuilder.AddColumn<bool>(
                name: "allow_mentions",
                table: "discord_guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_mentions",
                table: "discord_guilds");

            migrationBuilder.RenameColumn(
                name: "language_type",
                table: "discord_guilds",
                newName: "language");

            migrationBuilder.RenameColumn(
                name: "embed_color",
                table: "discord_guilds",
                newName: "prefix");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
