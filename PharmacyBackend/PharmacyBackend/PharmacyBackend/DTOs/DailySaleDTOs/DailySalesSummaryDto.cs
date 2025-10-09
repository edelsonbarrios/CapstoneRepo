namespace PharmacyBackend.DTOs.DailySaleDTOs
{
    public class DailySalesSummaryDto
    {
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public int TotalItemsSold { get; set; }
        public int UniqueProductsCount { get; set; }
    }
}
