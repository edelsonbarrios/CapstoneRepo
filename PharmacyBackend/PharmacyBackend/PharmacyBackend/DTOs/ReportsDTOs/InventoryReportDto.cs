using PharmacyBackend.DTOs.InventoryDTOs;

namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class InventoryReportDto
    {
        public InventoryReportSummaryDto Summary { get; set; }
        public IEnumerable<InventoryDto> InventoryItems { get; set; }
    }
}
