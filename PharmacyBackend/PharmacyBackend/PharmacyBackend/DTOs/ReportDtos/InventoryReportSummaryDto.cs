using PharmacyBackend.DTOs.InventoryDTOs;

namespace PharmacyBackend.DTOs.ReportDtos
{
    public class InventoryReportSummaryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FilterType { get; set; }
        public int TotalItems { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<InventoryDto> InventoryItems { get; set; }
    }
}
