using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Projeto4pServer.Migrations
{
    /// <inheritdoc />
    public partial class AlterationBlasphemy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Agendas",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         AgendaName = table.Column<string>(type: "text", nullable: false),
            //         NormalItem = table.Column<string>(type: "text", nullable: false),
            //         BoldItem = table.Column<string>(type: "text", nullable: false),
            //         SpecialRule = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Agendas", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Blasphemies",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         BlasphemyName = table.Column<string>(type: "text", nullable: false),
            //         Fact = table.Column<string>(type: "text", nullable: false),
            //         Passive = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Blasphemies", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "User",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         UserName = table.Column<string>(type: "text", nullable: false),
            //         Email = table.Column<string>(type: "text", nullable: false),
            //         PasswordHash = table.Column<string>(type: "text", nullable: false),
            //         ResetCode = table.Column<string>(type: "text", nullable: true),
            //         ResetCodeExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_User", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AgendaAbilities",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         AgendaId = table.Column<long>(type: "bigint", nullable: false),
            //         AbilityName = table.Column<string>(type: "text", nullable: false),
            //         Description = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AgendaAbilities", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AgendaAbilities_Agendas_AgendaId",
            //             column: x => x.AgendaId,
            //             principalTable: "Agendas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "BlasphemyAbilities",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         BlasphemyId = table.Column<long>(type: "bigint", nullable: false),
            //         AbilityName = table.Column<string>(type: "text", nullable: false),
            //         Description = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_BlasphemyAbilities", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_BlasphemyAbilities_Blasphemies_BlasphemyId",
            //             column: x => x.BlasphemyId,
            //             principalTable: "Blasphemies",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Characters",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //         Name = table.Column<string>(type: "text", nullable: false),
            //         CharacterXID = table.Column<string>(type: "text", nullable: false),
            //         Gender = table.Column<string>(type: "text", nullable: false),
            //         Height = table.Column<string>(type: "text", nullable: false),
            //         Weight = table.Column<string>(type: "text", nullable: false),
            //         HairColor = table.Column<string>(type: "text", nullable: false),
            //         EyeColor = table.Column<string>(type: "text", nullable: false),
            //         CAT = table.Column<int>(type: "integer", nullable: false),
            //         DivineAgony = table.Column<int>(type: "integer", nullable: false),
            //         Stress = table.Column<int>(type: "integer", nullable: false),
            //         Injury = table.Column<int>(type: "integer", nullable: false),
            //         XP = table.Column<int>(type: "integer", nullable: false),
            //         Advance = table.Column<int>(type: "integer", nullable: false),
            //         KitPoints = table.Column<int>(type: "integer", nullable: false),
            //         Burst = table.Column<int>(type: "integer", nullable: false),
            //         SinOverflow = table.Column<int>(type: "integer", nullable: false),
            //         Marks = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Characters", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Characters_User_UserId",
            //             column: x => x.UserId,
            //             principalTable: "User",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "CharacterSkills",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CharacterId = table.Column<long>(type: "bigint", nullable: false),
            //         Force = table.Column<int>(type: "integer", nullable: false),
            //         Conditioning = table.Column<int>(type: "integer", nullable: false),
            //         Coordination = table.Column<int>(type: "integer", nullable: false),
            //         Covert = table.Column<int>(type: "integer", nullable: false),
            //         Interfacing = table.Column<int>(type: "integer", nullable: false),
            //         Investigation = table.Column<int>(type: "integer", nullable: false),
            //         Authority = table.Column<int>(type: "integer", nullable: false),
            //         Surveillance = table.Column<int>(type: "integer", nullable: false),
            //         Negotiation = table.Column<int>(type: "integer", nullable: false),
            //         Connection = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_CharacterSkills", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_CharacterSkills_Characters_CharacterId",
            //             column: x => x.CharacterId,
            //             principalTable: "Characters",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "CharAgendas",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CharacterId = table.Column<long>(type: "bigint", nullable: false),
            //         AgendaAbilityId = table.Column<long>(type: "bigint", nullable: false),
            //         AgendaId = table.Column<long>(type: "bigint", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_CharAgendas", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_CharAgendas_AgendaAbilities_AgendaAbilityId",
            //             column: x => x.AgendaAbilityId,
            //             principalTable: "AgendaAbilities",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_CharAgendas_Agendas_AgendaId",
            //             column: x => x.AgendaId,
            //             principalTable: "Agendas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_CharAgendas_Characters_CharacterId",
            //             column: x => x.CharacterId,
            //             principalTable: "Characters",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "CharBlasphemies",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CharacterId = table.Column<long>(type: "bigint", nullable: false),
            //         BlasphemyAbilityId = table.Column<long>(type: "bigint", nullable: false),
            //         BlasphemyId = table.Column<long>(type: "bigint", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_CharBlasphemies", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_CharBlasphemies_Blasphemies_BlasphemyId",
            //             column: x => x.BlasphemyId,
            //             principalTable: "Blasphemies",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_CharBlasphemies_BlasphemyAbilities_BlasphemyAbilityId",
            //             column: x => x.BlasphemyAbilityId,
            //             principalTable: "BlasphemyAbilities",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_CharBlasphemies_Characters_CharacterId",
            //             column: x => x.CharacterId,
            //             principalTable: "Characters",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Inventories",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CharacterId = table.Column<long>(type: "bigint", nullable: false),
            //         ItemName = table.Column<string>(type: "text", nullable: false),
            //         ItemDescription = table.Column<string>(type: "text", nullable: true),
            //         Quantity = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Inventories", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Inventories_Characters_CharacterId",
            //             column: x => x.CharacterId,
            //             principalTable: "Characters",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_AgendaAbilities_AgendaId",
            //     table: "AgendaAbilities",
            //     column: "AgendaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_BlasphemyAbilities_BlasphemyId",
            //     table: "BlasphemyAbilities",
            //     column: "BlasphemyId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Characters_UserId",
            //     table: "Characters",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharacterSkills_CharacterId",
            //     table: "CharacterSkills",
            //     column: "CharacterId",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharAgendas_AgendaAbilityId",
            //     table: "CharAgendas",
            //     column: "AgendaAbilityId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharAgendas_AgendaId",
            //     table: "CharAgendas",
            //     column: "AgendaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharAgendas_CharacterId",
            //     table: "CharAgendas",
            //     column: "CharacterId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharBlasphemies_BlasphemyAbilityId",
            //     table: "CharBlasphemies",
            //     column: "BlasphemyAbilityId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharBlasphemies_BlasphemyId",
            //     table: "CharBlasphemies",
            //     column: "BlasphemyId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CharBlasphemies_CharacterId",
            //     table: "CharBlasphemies",
            //     column: "CharacterId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Inventories_CharacterId",
            //     table: "Inventories",
            //     column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSkills");

            migrationBuilder.DropTable(
                name: "CharAgendas");

            migrationBuilder.DropTable(
                name: "CharBlasphemies");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "AgendaAbilities");

            migrationBuilder.DropTable(
                name: "BlasphemyAbilities");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "Blasphemies");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
