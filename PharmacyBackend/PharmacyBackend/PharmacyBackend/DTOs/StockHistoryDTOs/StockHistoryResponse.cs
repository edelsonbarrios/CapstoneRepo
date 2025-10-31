namespace PharmacyBackend.DTOs.StockHistoryDTOs
{
    public class StockHistoryResponse
    {
        public StockHistorySummaryDto Summary { get; set; } = new StockHistorySummaryDto();
        public List<StockHistoryDto> Transactions { get; set; } = new List<StockHistoryDto>();
    }
}
