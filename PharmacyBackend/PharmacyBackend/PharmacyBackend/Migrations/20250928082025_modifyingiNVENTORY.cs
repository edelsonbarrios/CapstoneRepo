using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class modifyingiNVENTORY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityChanged",
                table: "StockMovement",
                newName: "SheetQuantityChanged");

            migrationBuilder.RenameColumn(
                name: "QuantityBefore",
                table: "StockMovement",
                newName: "SheetQuantityBefore");

            migrationBuilder.RenameColumn(
                name: "QuantityAfter",
                table: "StockMovement",
                newName: "SheetQuantityAfter");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Inventory",
                newName: "BoxPrice");

            migrationBuilder.RenameColumn(
                name: "QuantityOnHand",
                table: "Inventory",
                newName: "SheetQuantity");

            migrationBuilder.AddColumn<int>(
                name: "BoxQuantityAfter",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoxQuantityBefore",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoxQuantityChanged",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PieceQuantityAfter",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PieceQuantityBefore",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PieceQuantityChanged",
                table: "StockMovement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitType",
                table: "SalesItem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoxQuantity",
                table: "Inventory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PiecePrice",
                table: "Inventory",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PieceQuantity",
                table: "Inventory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PiecesPerSheet",
                table: "Inventory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SheetPrice",
                table: "Inventory",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SheetsPerBox",
                table: "Inventory",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxQuantityAfter",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "BoxQuantityBefore",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "BoxQuantityChanged",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "PieceQuantityAfter",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "PieceQuantityBefore",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "PieceQuantityChanged",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "SalesItem");

            migrationBuilder.DropColumn(
                name: "BoxQuantity",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PiecePrice",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PieceQuantity",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PiecesPerSheet",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "SheetPrice",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "SheetsPerBox",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "SheetQuantityChanged",
                table: "StockMovement",
                newName: "QuantityChanged");

            migrationBuilder.RenameColumn(
                name: "SheetQuantityBefore",
                table: "StockMovement",
                newName: "QuantityBefore");

            migrationBuilder.RenameColumn(
                name: "SheetQuantityAfter",
                table: "StockMovement",
                newName: "QuantityAfter");

            migrationBuilder.RenameColumn(
                name: "SheetQuantity",
                table: "Inventory",
                newName: "QuantityOnHand");

            migrationBuilder.RenameColumn(
                name: "BoxPrice",
                table: "Inventory",
                newName: "SellingPrice");
        }
    }
}
