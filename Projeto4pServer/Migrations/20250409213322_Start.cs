using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Projeto4pServer.Migrations
{
    /// <inheritdoc />
    public partial class Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CharacterXID = table.Column<string>(type: "text", nullable: false),
                    Agenda = table.Column<string>(type: "text", nullable: false),
                    Blasfemia = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Heigth = table.Column<string>(type: "text", nullable: false),
                    Weigth = table.Column<string>(type: "text", nullable: false),
                    HairColor = table.Column<string>(type: "text", nullable: false),
                    EyeColor = table.Column<string>(type: "text", nullable: false),
                    CAT = table.Column<int>(type: "integer", nullable: false),
                    DivineAgony = table.Column<int>(type: "integer", nullable: false),
                    Injury = table.Column<int>(type: "integer", nullable: false),
                    Stress = table.Column<int>(type: "integer", nullable: false),
                    XP = table.Column<int>(type: "integer", nullable: false),
                    Advance = table.Column<int>(type: "integer", nullable: false),
                    KitPoints = table.Column<int>(type: "integer", nullable: false),
                    Burst = table.Column<int>(type: "integer", nullable: false),
                    SinOverflow = table.Column<int>(type: "integer", nullable: false),
                    Marks = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    ResetCode = table.Column<string>(type: "text", nullable: true),
                    ResetCodeExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    CharacterID = table.Column<long>(type: "bigint", nullable: false),
                    Items = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.CharacterID);
                    table.ForeignKey(
                        name: "FK_Inventory_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    CharacterID = table.Column<long>(type: "bigint", nullable: false),
                    Force = table.Column<int>(type: "integer", nullable: false),
                    Conditioning = table.Column<int>(type: "integer", nullable: false),
                    Coordination = table.Column<int>(type: "integer", nullable: false),
                    Covert = table.Column<int>(type: "integer", nullable: false),
                    Interfacing = table.Column<int>(type: "integer", nullable: false),
                    Investigation = table.Column<int>(type: "integer", nullable: false),
                    Surveillance = table.Column<int>(type: "integer", nullable: false),
                    Negotiation = table.Column<int>(type: "integer", nullable: false),
                    Authority = table.Column<int>(type: "integer", nullable: false),
                    Connection = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.CharacterID);
                    table.ForeignKey(
                        name: "FK_Skills_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
