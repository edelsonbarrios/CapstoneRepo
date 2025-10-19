using PharmacyBackend.DTOs.SalesDTOs;

namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class SalesReportDto
    {
        public SalesReportSummaryDto Summary { get; set; }
        public IEnumerable<SalesTransactionDto> Transactions { get; set; }
    }
}
