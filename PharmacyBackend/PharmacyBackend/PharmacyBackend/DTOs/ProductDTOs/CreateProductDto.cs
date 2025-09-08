//DTOs/CreateProductDto.cs
namespace PharmacyBackend.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string FormulationName { get; set; } = string.Empty;
        public string ClassificationName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string? SupplierName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
