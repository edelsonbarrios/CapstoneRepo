using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingPiecePerBoxProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPiecesAvailable",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "PiecePerBox",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PiecePerBox",
                table: "Inventory",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PiecePerBox",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PiecePerBox",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "TotalPiecesAvailable",
                table: "Inventory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
