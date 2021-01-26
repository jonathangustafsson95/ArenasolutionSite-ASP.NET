using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class TournamentChanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Knockouts_CurrentRoundKnockoutId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_CurrentRoundKnockoutId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CurrentRoundKnockoutId",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "KnockoutId",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_KnockoutId",
                table: "Tournaments",
                column: "KnockoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments",
                column: "KnockoutId",
                principalTable: "Knockouts",
                principalColumn: "KnockoutId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_KnockoutId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "KnockoutId",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "CurrentRoundKnockoutId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CurrentRoundKnockoutId",
                table: "Tournaments",
                column: "CurrentRoundKnockoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Knockouts_CurrentRoundKnockoutId",
                table: "Tournaments",
                column: "CurrentRoundKnockoutId",
                principalTable: "Knockouts",
                principalColumn: "KnockoutId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
