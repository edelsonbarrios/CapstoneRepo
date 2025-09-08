//DTOs/CreateProductDto.cs
namespace PharmacyBackend.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public int GenericID { get; set; }
        public int FormulationID { get; set; }
        public int ClassificationID { get; set; }
        public int TypeID { get; set; }
        public int SupplierID { get; set; }
        public decimal Price { get; set; }
    }
}
