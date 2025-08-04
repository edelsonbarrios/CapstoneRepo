namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class UpdateSalesItemDto
    {
        public int Id { get; set; } //reference the existing sales item
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
