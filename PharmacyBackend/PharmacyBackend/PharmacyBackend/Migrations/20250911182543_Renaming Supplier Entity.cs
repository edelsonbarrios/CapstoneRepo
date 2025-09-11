using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamingSupplierEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierID",
                table: "Supplier",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderItemID",
                table: "PurchaseOrderItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderID",
                table: "PurchaseOrder",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Supplier",
                newName: "SupplierID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseOrderItem",
                newName: "PurchaseOrderItemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseOrder",
                newName: "PurchaseOrderID");
        }
    }
}
