using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class RemoveAllowMentionsProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_mentions",
                table: "discord_guilds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_mentions",
                table: "discord_guilds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
