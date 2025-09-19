using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangingEntityNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Classifications_ClassificationID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Formulations_FormulationID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Generics_GenericID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeModels_TypeID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItem_Products_ProductId",
                table: "SalesItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeModels",
                table: "TypeModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Generics",
                table: "Generics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Formulations",
                table: "Formulations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "TypeModels",
                newName: "TypeModel");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Generics",
                newName: "Generic");

            migrationBuilder.RenameTable(
                name: "Formulations",
                newName: "Formulation");

            migrationBuilder.RenameTable(
                name: "Classifications",
                newName: "Classification");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameIndex(
                name: "IX_Products_TypeID",
                table: "Product",
                newName: "IX_Product_TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_GenericID",
                table: "Product",
                newName: "IX_Product_GenericID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_FormulationID",
                table: "Product",
                newName: "IX_Product_FormulationID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ClassificationID",
                table: "Product",
                newName: "IX_Product_ClassificationID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandID",
                table: "Product",
                newName: "IX_Product_BrandID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeModel",
                table: "TypeModel",
                column: "TypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Generic",
                table: "Generic",
                column: "GenericID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Formulation",
                table: "Formulation",
                column: "FormulationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classification",
                table: "Classification",
                column: "ClassificationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "BrandID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItem_Product_ProductId",
                table: "SalesItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItem_Product_ProductId",
                table: "SalesItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeModel",
                table: "TypeModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Generic",
                table: "Generic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Formulation",
                table: "Formulation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classification",
                table: "Classification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.RenameTable(
                name: "TypeModel",
                newName: "TypeModels");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Generic",
                newName: "Generics");

            migrationBuilder.RenameTable(
                name: "Formulation",
                newName: "Formulations");

            migrationBuilder.RenameTable(
                name: "Classification",
                newName: "Classifications");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameIndex(
                name: "IX_Product_TypeID",
                table: "Products",
                newName: "IX_Products_TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_GenericID",
                table: "Products",
                newName: "IX_Products_GenericID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_FormulationID",
                table: "Products",
                newName: "IX_Products_FormulationID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ClassificationID",
                table: "Products",
                newName: "IX_Products_ClassificationID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_BrandID",
                table: "Products",
                newName: "IX_Products_BrandID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeModels",
                table: "TypeModels",
                column: "TypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Generics",
                table: "Generics",
                column: "GenericID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Formulations",
                table: "Formulations",
                column: "FormulationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications",
                column: "ClassificationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "BrandID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Classifications_ClassificationID",
                table: "Products",
                column: "ClassificationID",
                principalTable: "Classifications",
                principalColumn: "ClassificationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Formulations_FormulationID",
                table: "Products",
                column: "FormulationID",
                principalTable: "Formulations",
                principalColumn: "FormulationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Generics_GenericID",
                table: "Products",
                column: "GenericID",
                principalTable: "Generics",
                principalColumn: "GenericID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeModels_TypeID",
                table: "Products",
                column: "TypeID",
                principalTable: "TypeModels",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItem_Products_ProductId",
                table: "SalesItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
