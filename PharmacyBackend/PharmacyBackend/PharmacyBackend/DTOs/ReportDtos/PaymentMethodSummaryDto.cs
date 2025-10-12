namespace PharmacyBackend.DTOs.ReportDtos
{
    public class PaymentMethodSummaryDto
    {
        public string PaymentMethod { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public decimal Percentage { get; set; }
    }
}


