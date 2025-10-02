//DTOs/SalesDTOs/CreateSalesItemDto.cs
namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class CreateSalesItemDto
    {
        public int ProductId { get; set; }
        public string UnitType { get; set; } = "Piece"; // "Box", "Sheet", "Piece"
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int SalesTransactionId { get; set; }
    }
}
