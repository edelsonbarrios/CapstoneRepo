namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class FastMovingProductDto
    {
        public int Rank { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Generic { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string UnitType { get; set; } = string.Empty;
        public int? BoxSold { get; set; }
        public int? SheetSold { get; set; }
        public int PieceSold { get; set; }
        public int QuantitySold { get; set; } // ADD THIS LINE
        public int TransactionCount { get; set; }
        public decimal Revenue { get; set; }
        public int CurrentStock { get; set; }
        public string StockStatus { get; set; } = string.Empty;
        public string Trend { get; set; } = string.Empty;
    }
}
