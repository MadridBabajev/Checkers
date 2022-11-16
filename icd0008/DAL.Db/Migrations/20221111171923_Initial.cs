using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Db.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckersOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WhitesFirst = table.Column<bool>(type: "INTEGER", nullable: false),
                    MandatoryTake = table.Column<bool>(type: "INTEGER", nullable: false),
                    QueensHaveOpMoves = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoardWidth = table.Column<short>(type: "INTEGER", nullable: false),
                    BoardHeight = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckersOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    PlayerType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckersGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameOverAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GameType = table.Column<int>(type: "INTEGER", nullable: false),
                    GameWonByPlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    GamePlayer1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    GamePlayer2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckersOptionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckersGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckersGames_CheckersOptions_CheckersOptionsId",
                        column: x => x.CheckersOptionsId,
                        principalTable: "CheckersOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckersGames_Players_GamePlayer1Id",
                        column: x => x.GamePlayer1Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckersGames_Players_GamePlayer2Id",
                        column: x => x.GamePlayer2Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckersGames_Players_GameWonByPlayerId",
                        column: x => x.GameWonByPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SerializedGameState = table.Column<string>(type: "TEXT", nullable: false),
                    CheckersGameId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStates_CheckersGames_CheckersGameId",
                        column: x => x.CheckersGameId,
                        principalTable: "CheckersGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_CheckersOptionsId",
                table: "CheckersGames",
                column: "CheckersOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_GamePlayer1Id",
                table: "CheckersGames",
                column: "GamePlayer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_GamePlayer2Id",
                table: "CheckersGames",
                column: "GamePlayer2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckersGames_GameWonByPlayerId",
                table: "CheckersGames",
                column: "GameWonByPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_CheckersGameId",
                table: "GameStates",
                column: "CheckersGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerName",
                table: "Players",
                column: "PlayerName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "CheckersGames");

            migrationBuilder.DropTable(
                name: "CheckersOptions");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
