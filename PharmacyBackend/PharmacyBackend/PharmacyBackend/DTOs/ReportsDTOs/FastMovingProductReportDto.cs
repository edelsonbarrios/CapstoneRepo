namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class FastMovingProductReportDto
    {
        public int TotalProductsSold { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public IEnumerable<FastMovingProductDto> Products { get; set; }
    }
}
