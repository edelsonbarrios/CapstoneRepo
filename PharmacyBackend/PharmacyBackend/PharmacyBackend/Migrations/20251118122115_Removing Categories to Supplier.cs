using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemovingCategoriestoSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Supplier_SupplierId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_SupplierId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Category",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_SupplierId",
                table: "Category",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Supplier_SupplierId",
                table: "Category",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id");
        }
    }
}
