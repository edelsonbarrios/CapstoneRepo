//DTOs/SalesDTOs/UpdateSalesItemDto.cs
namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class UpdateSalesItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
