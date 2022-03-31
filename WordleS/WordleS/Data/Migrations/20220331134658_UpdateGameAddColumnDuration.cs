using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordleS.Data.Migrations
{
    public partial class UpdateGameAddColumnDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Game",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Game");
        }
    }
}
