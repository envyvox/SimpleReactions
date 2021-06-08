using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SR.Data.Migrations
{
    public partial class DiscordGuildModelInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "type",
                table: "reactions",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "discord_guilds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    prefix = table.Column<string>(type: "text", nullable: false, defaultValue: "."),
                    language = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discord_guilds", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discord_guilds");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "reactions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
