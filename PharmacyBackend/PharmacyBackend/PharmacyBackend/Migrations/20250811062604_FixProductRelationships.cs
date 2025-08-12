using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixProductRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandID",
                table: "Product",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ClassificationID",
                table: "Product",
                column: "ClassificationID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FormulationID",
                table: "Product",
                column: "FormulationID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_GenericID",
                table: "Product",
                column: "GenericID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TypeID",
                table: "Product",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Brand_BrandID",
                table: "Product",
                column: "BrandID",
                principalTable: "Brand",
                principalColumn: "BrandID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Classification_ClassificationID",
                table: "Product",
                column: "ClassificationID",
                principalTable: "Classification",
                principalColumn: "ClassificationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Formulation_FormulationID",
                table: "Product",
                column: "FormulationID",
                principalTable: "Formulation",
                principalColumn: "FormulationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Generic_GenericID",
                table: "Product",
                column: "GenericID",
                principalTable: "Generic",
                principalColumn: "GenericID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_TypeModel_TypeID",
                table: "Product",
                column: "TypeID",
                principalTable: "TypeModel",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Brand_BrandID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Classification_ClassificationID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Formulation_FormulationID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Generic_GenericID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_TypeModel_TypeID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_BrandID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ClassificationID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_FormulationID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_GenericID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_TypeID",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
