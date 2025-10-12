namespace PharmacyBackend.DTOs.ReportDtos
{
    public class SalesReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal NetAmount { get; set; }
        public List<PaymentMethodSummaryDto> PaymentMethodBreakdown { get; set; }
    }
}
