using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class UpdateDiscordGuildAddColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "prefix",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: "-",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: ".");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: "36393F");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "discord_guilds");

            migrationBuilder.AlterColumn<string>(
                name: "prefix",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: ".",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "-");
        }
    }
}
