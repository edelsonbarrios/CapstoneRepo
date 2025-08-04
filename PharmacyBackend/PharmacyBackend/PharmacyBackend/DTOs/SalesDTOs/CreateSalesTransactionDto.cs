namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class CreateSalesTransactionDto
    {
        public string InvoiceNumber { get; set; }
        public int? DiscountId { get; set; } // New

        public List<CreateSalesItemDto> Items { get; set; } = new();
    }
}
