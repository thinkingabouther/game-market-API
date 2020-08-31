using Microsoft.EntityFrameworkCore.Migrations;

namespace game_market_API.Migrations
{
    public partial class addIsCompletedpropertyforsession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "PaymentSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "PaymentSessions");
        }
    }
}
