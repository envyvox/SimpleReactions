using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class UpdateDiscordGuildNewDefaultPrefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "prefix",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: "..",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "-");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "prefix",
                table: "discord_guilds",
                type: "text",
                nullable: false,
                defaultValue: "-",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "..");
        }
    }
}
