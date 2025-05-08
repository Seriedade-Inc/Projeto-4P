using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto4pServer.Migrations
{
    /// <inheritdoc />
    public partial class AddItemDescriptionToInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "Inventories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "Inventories");
        }
    }
}
