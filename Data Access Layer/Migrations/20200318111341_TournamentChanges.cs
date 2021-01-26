using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class TournamentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentRoundKnockoutId",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Knockouts",
                columns: table => new
                {
                    KnockoutId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeftNodeKnockoutId = table.Column<int>(nullable: true),
                    RightNodeKnockoutId = table.Column<int>(nullable: true),
                    player1TournamentId = table.Column<int>(nullable: true),
                    player1UserId = table.Column<int>(nullable: true),
                    player2TournamentId = table.Column<int>(nullable: true),
                    player2UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knockouts", x => x.KnockoutId);
                    table.ForeignKey(
                        name: "FK_Knockouts_Knockouts_LeftNodeKnockoutId",
                        column: x => x.LeftNodeKnockoutId,
                        principalTable: "Knockouts",
                        principalColumn: "KnockoutId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Knockouts_Knockouts_RightNodeKnockoutId",
                        column: x => x.RightNodeKnockoutId,
                        principalTable: "Knockouts",
                        principalColumn: "KnockoutId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Knockouts_TournamentPlayers_player1TournamentId_player1UserId",
                        columns: x => new { x.player1TournamentId, x.player1UserId },
                        principalTable: "TournamentPlayers",
                        principalColumns: new[] { "TournamentId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Knockouts_TournamentPlayers_player2TournamentId_player2UserId",
                        columns: x => new { x.player2TournamentId, x.player2UserId },
                        principalTable: "TournamentPlayers",
                        principalColumns: new[] { "TournamentId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CurrentRoundKnockoutId",
                table: "Tournaments",
                column: "CurrentRoundKnockoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Knockouts_LeftNodeKnockoutId",
                table: "Knockouts",
                column: "LeftNodeKnockoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Knockouts_RightNodeKnockoutId",
                table: "Knockouts",
                column: "RightNodeKnockoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Knockouts_player1TournamentId_player1UserId",
                table: "Knockouts",
                columns: new[] { "player1TournamentId", "player1UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Knockouts_player2TournamentId_player2UserId",
                table: "Knockouts",
                columns: new[] { "player2TournamentId", "player2UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Knockouts_CurrentRoundKnockoutId",
                table: "Tournaments",
                column: "CurrentRoundKnockoutId",
                principalTable: "Knockouts",
                principalColumn: "KnockoutId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Knockouts_CurrentRoundKnockoutId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Knockouts");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_CurrentRoundKnockoutId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CurrentRoundKnockoutId",
                table: "Tournaments");
        }
    }
}
