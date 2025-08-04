namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class CreateSalesItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
