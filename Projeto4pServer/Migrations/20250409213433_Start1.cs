using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto4pServer.Migrations
{
    /// <inheritdoc />
    public partial class Start1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpiration",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiration",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
