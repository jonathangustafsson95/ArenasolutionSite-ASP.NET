using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class tournamentChanges3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<int>(
                name: "KnockoutId",
                table: "Tournaments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments",
                column: "KnockoutId",
                principalTable: "Knockouts",
                principalColumn: "KnockoutId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<int>(
                name: "KnockoutId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Knockouts_KnockoutId",
                table: "Tournaments",
                column: "KnockoutId",
                principalTable: "Knockouts",
                principalColumn: "KnockoutId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
