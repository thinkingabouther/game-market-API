using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace game_market_API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentSessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSessions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    VendorID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_Users_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameKeys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    IsActivated = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameID = table.Column<int>(type: "INTEGER", nullable: true),
                    ClientID = table.Column<int>(type: "INTEGER", nullable: true),
                    PaymentSessionID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameKeys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameKeys_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameKeys_PaymentSessions_PaymentSessionID",
                        column: x => x.PaymentSessionID,
                        principalTable: "PaymentSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameKeys_Users_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Password", "Role", "Username" },
                values: new object[] { -2, "12345", "Vendor", "Vasya" });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "Name", "Price", "VendorID" },
                values: new object[] { -1, "Dota 2", 99.0, -2 });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "Name", "Price", "VendorID" },
                values: new object[] { -2, "Microsoft Flight Simulator", 4356.0, -2 });

            migrationBuilder.CreateIndex(
                name: "IX_GameKeys_ClientID",
                table: "GameKeys",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_GameKeys_GameID",
                table: "GameKeys",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_GameKeys_PaymentSessionID",
                table: "GameKeys",
                column: "PaymentSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_VendorID",
                table: "Games",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameKeys");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PaymentSessions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
