using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "discord_guilds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    prefix = table.Column<string>(type: "text", nullable: false),
                    language = table.Column<byte>(type: "smallint", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discord_guilds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reactions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_discord_guilds_id",
                table: "discord_guilds",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_type_url",
                table: "reactions",
                columns: new[] { "type", "url" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discord_guilds");

            migrationBuilder.DropTable(
                name: "reactions");
        }
    }
}
