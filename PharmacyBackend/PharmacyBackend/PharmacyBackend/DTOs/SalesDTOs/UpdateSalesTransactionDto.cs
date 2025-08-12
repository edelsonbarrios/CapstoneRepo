namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class UpdateSalesTransactionDto
    {
        public int? DiscountId { get; set; }
        public decimal DiscountAmount { get; set; } 
        public List<UpdateSalesItemDto> Items { get; set; } = new();
    }
}