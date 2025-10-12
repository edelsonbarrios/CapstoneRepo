namespace PharmacyBackend.DTOs.ReportDtos
{
    public class FastMovingProductsReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public int TopN { get; set; }
        public int MinimumThreshold { get; set; }
        public List<FastMovingProductDto> Products { get; set; }
    }
}
