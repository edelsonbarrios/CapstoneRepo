namespace PharmacyBackend.DTOs.DailySaleDTOs
{
    public class ProductSalesSummaryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public int AvailableStock { get; set; }
        public decimal TotalSales { get; set; }

        // Add these new properties
        public string UnitMeasurement { get; set; }
        public int? BoxSold { get; set; }
        public int? SheetSold { get; set; }
        public int? PieceSold { get; set; }
    }
}
