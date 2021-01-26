using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class league123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueMemberId",
                table: "LeagueMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueMemberId",
                table: "LeagueMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
