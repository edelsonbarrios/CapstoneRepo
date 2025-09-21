//DTOs/SalesDTOs/SalesTransactionDto.cs
namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class SalesTransactionDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeAmount { get; set; }
        public int DiscountId { get; set; }
        public int EmployeeID { get; set; }
        public int? VATId { get; set; }

        // Navigation properties for display
        public string DiscountName { get; set; }
        public string EmployeeName { get; set; }
        public decimal? VATRate { get; set; }

        public List<SalesItemDto> Items { get; set; } = new List<SalesItemDto>();
    }
}
