using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class league : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TournamentStyles",
                columns: table => new
                {
                    TournamentStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentStyles = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStyles", x => x.TournamentStyleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 10, nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Balance = table.Column<float>(nullable: true),
                    Admin = table.Column<bool>(nullable: true),
                    Rating = table.Column<int>(nullable: true),
                    ConnectionId = table.Column<string>(nullable: true),
                    CurrentGameType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Adverts",
                columns: table => new
                {
                    advertId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertName = table.Column<string>(maxLength: 30, nullable: false),
                    Sponsoring = table.Column<string>(maxLength: 30, nullable: false),
                    BeginDateTime = table.Column<DateTime>(nullable: false),
                    DeadlineDateTime = table.Column<DateTime>(nullable: false),
                    productImage = table.Column<byte[]>(nullable: true),
                    Link = table.Column<string>(maxLength: 500, nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adverts", x => x.advertId);
                    table.ForeignKey(
                        name: "FK_Adverts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    LeagueId = table.Column<string>(nullable: false),
                    LeagueOwnerId = table.Column<int>(nullable: false),
                    LeagueName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.LeagueId);
                    table.ForeignKey(
                        name: "FK_Leagues_Users_LeagueOwnerId",
                        column: x => x.LeagueOwnerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueMembers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LeagueId = table.Column<string>(nullable: false),
                    LeagueMemberId = table.Column<int>(nullable: false),
                    Applicant = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueMembers", x => new { x.LeagueId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LeagueMembers_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueId");
                    table.ForeignKey(
                        name: "FK_LeagueMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(nullable: false),
                    LeagueId1 = table.Column<string>(nullable: true),
                    TournamentName = table.Column<string>(nullable: false),
                    TournamentStyle = table.Column<string>(nullable: false),
                    MaxPlayer = table.Column<int>(nullable: false),
                    CurrentPlayers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                    table.ForeignKey(
                        name: "FK_Tournaments_Leagues_LeagueId1",
                        column: x => x.LeagueId1,
                        principalTable: "Leagues",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPlayers",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    TournamentPlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPlayers", x => new { x.TournamentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TournamentPlayers_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId");
                    table.ForeignKey(
                        name: "FK_TournamentPlayers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Discriminator", "Password", "UserName", "Admin" },
                values: new object[] { 1, "Operator", "123", "Jonte", false });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_UserId",
                table: "Adverts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMembers_UserId",
                table: "LeagueMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_LeagueOwnerId",
                table: "Leagues",
                column: "LeagueOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayers_UserId",
                table: "TournamentPlayers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_LeagueId1",
                table: "Tournaments",
                column: "LeagueId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adverts");

            migrationBuilder.DropTable(
                name: "LeagueMembers");

            migrationBuilder.DropTable(
                name: "TournamentPlayers");

            migrationBuilder.DropTable(
                name: "TournamentStyles");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
