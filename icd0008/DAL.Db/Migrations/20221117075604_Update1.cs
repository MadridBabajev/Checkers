using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Db.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "BlacksLeft",
                table: "CheckersGames",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "WhitesLeft",
                table: "CheckersGames",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlacksLeft",
                table: "CheckersGames");

            migrationBuilder.DropColumn(
                name: "WhitesLeft",
                table: "CheckersGames");
        }
    }
}
