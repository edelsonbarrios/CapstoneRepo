using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class InventoryDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Inventory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_DiscountId",
                table: "Inventory",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Discounts_DiscountId",
                table: "Inventory",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Discounts_DiscountId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_DiscountId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Inventory");
        }
    }
}
