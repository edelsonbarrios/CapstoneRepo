namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class InventoryReportSummaryDto
    {
        public int TotalProducts { get; set; }
        public decimal TotalValue { get; set; }
        public int CriticalStockCount { get; set; }
        public int NearExpiryCount { get; set; }
    }
}
