using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsActivepropertytoItemDataControls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TypeModel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Generic",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Formulation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Classification",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brand",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TypeModel");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Generic");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Formulation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Classification");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brand");
        }
    }
}
