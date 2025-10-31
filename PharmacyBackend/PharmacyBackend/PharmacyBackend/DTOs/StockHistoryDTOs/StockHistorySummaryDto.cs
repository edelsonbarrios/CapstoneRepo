namespace PharmacyBackend.DTOs.StockHistoryDTOs
{
    public class StockHistorySummaryDto
    {
        public int TotalTransactions { get; set; }
        public int StockEntries { get; set; }
        public int Adjustments { get; set; }
        public int SalesRemovals { get; set; }
    }
}
