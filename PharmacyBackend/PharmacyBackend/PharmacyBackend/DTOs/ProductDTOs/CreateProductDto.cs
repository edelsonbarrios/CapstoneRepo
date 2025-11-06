//DTOs/CreateProductDto.cs
namespace PharmacyBackend.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? BrandName { get; set; } = string.Empty;
        public string? GenericName { get; set; } = string.Empty;
        public string? FormulationName { get; set; } = string.Empty;
        public string? ClassificationName { get; set; } = string.Empty;
        public string? TypeName { get; set; } = string.Empty;
        public int SheetsPerBox { get; set; }
        public int PiecesPerSheet { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }
        public int ReorderLevel { get; set; }
        public string Description { get; set; } = string.Empty;
        public string UnitMeasurement { get; set; }
        public int PiecePerBox { get; set; }
        public int CostPrice {get; set;}
        public decimal CostBoxPrice {get; set;}
        public decimal CostSheetPrice {get; set;}
        public decimal CostPiecePrice {get; set;}
    }
}
