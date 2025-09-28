using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AlteringProductSupplierEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "ProductSupplier",
                columns: table => new
                {
                    ProductsProductID = table.Column<int>(type: "integer", nullable: false),
                    SuppliersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSupplier", x => new { x.ProductsProductID, x.SuppliersId });
                    table.ForeignKey(
                        name: "FK_ProductSupplier_Product_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSupplier_Supplier_SuppliersId",
                        column: x => x.SuppliersId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSupplier_SuppliersId",
                table: "ProductSupplier",
                column: "SuppliersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSupplier");

            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierID",
                table: "Product",
                column: "SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product",
                column: "SupplierID",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
