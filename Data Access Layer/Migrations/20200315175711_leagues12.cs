using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class leagues12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Leagues_LeagueId1",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_LeagueId1",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "LeagueId1",
                table: "Tournaments");

            migrationBuilder.AlterColumn<string>(
                name: "LeagueId",
                table: "Tournaments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_LeagueId",
                table: "Tournaments",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Leagues_LeagueId",
                table: "Tournaments",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Leagues_LeagueId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_LeagueId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<int>(
                name: "LeagueId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "LeagueId1",
                table: "Tournaments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_LeagueId1",
                table: "Tournaments",
                column: "LeagueId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Leagues_LeagueId1",
                table: "Tournaments",
                column: "LeagueId1",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
