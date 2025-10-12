namespace PharmacyBackend.DTOs.ReportDtos
{
    public class FastMovingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }

        // Quantity metrics (converted to pieces for consistency)
        public int TotalPiecesSold { get; set; }
        public int TotalBoxesSold { get; set; }
        public int TotalSheetsSold { get; set; }

        // Transaction metrics
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }

        // Time-based metrics
        public int DaysInPeriod { get; set; }
        public decimal AveragePiecesPerDay { get; set; }
        public decimal AverageRevenuePerDay { get; set; }

        // Ranking
        public int Rank { get; set; }
    }
}
