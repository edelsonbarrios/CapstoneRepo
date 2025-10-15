using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingContactPersonNumbertoSupplierEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPersonNumber",
                table: "Supplier",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPersonNumber",
                table: "Supplier");
        }
    }
}
