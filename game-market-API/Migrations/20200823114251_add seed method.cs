using Microsoft.EntityFrameworkCore.Migrations;

namespace game_market_API.Migrations
{
    public partial class addseedmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "Name", "Price" },
                values: new object[] { 1, "Dota 2", 99.0 });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "Name", "Price" },
                values: new object[] { 2, "Microsoft Flight Simulator", 4356.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
