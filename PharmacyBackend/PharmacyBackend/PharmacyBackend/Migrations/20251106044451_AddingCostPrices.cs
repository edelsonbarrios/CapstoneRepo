using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingCostPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostBoxPrice",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPiecePrice",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostSheetPrice",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostBoxPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CostPiecePrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CostSheetPrice",
                table: "Product");
        }
    }
}
