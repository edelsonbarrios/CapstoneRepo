namespace PharmacyBackend.DTOs.StockHistoryDTOs
{
    public class StockHistoryFilterRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TransactionType { get; set; } // "All Transaction", "Stock Entry", "Adjustment", "Sale", "Damaged", "Expired"
        public string? SearchTerm { get; set; } // Search by product name, batch number, or reference number
    }
}