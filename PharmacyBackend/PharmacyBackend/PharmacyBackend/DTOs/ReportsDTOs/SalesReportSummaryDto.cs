namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class SalesReportSummaryDto
    {
        public int TotalTransactions { get; set; }
        public decimal NetRevenue { get; set; }
        public int TotalItemsSold { get; set; }
        public decimal AverageTransaction { get; set; }
    }
}
