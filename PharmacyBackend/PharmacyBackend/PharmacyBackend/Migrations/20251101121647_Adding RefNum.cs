using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingRefNum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "SalesTransaction",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "SalesTransaction");
        }
    }
}
