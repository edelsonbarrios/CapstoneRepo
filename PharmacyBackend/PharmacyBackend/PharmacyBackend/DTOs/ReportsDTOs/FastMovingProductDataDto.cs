namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class FastMovingProductDataDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Generic { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string UnitType { get; set; } = string.Empty;
        public int? SheetsPerBox { get; set; }
        public int? PiecesPerSheet { get; set; }
        public int? PiecePerBox { get; set; }
        public int QuantitySold { get; set; }
        public int TransactionCount { get; set; }
        public decimal Revenue { get; set; }
        public int CurrentStock { get; set; }
    }
}
