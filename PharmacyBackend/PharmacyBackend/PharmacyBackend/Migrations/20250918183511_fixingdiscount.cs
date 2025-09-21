using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class fixingdiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SalesItem");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "SalesTransaction",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ChangeAmount",
                table: "SalesTransaction",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "SalesTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VATAmount",
                table: "SalesTransaction",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "VATId",
                table: "SalesTransaction",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransaction_VATId",
                table: "SalesTransaction",
                column: "VATId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransaction_VAT_VATId",
                table: "SalesTransaction",
                column: "VATId",
                principalTable: "VAT",
                principalColumn: "VATID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransaction_VAT_VATId",
                table: "SalesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransaction_VATId",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "ChangeAmount",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "VATAmount",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "VATId",
                table: "SalesTransaction");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SalesItem",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
