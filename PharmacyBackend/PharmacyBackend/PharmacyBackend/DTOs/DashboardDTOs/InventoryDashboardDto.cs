using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ProductDTOs;

namespace PharmacyBackend.DTOs.DashboardDTOs
{
    public class InventoryDashboardDto
    {
        public int TotalActiveProducts { get; set; }
        public int TotalInventoryItems { get; set; }
        public int CriticalStockCount { get; set; }
        public int NearExpiryCount { get; set; }
        public int ExpiredStockCount { get; set; }

        public List<InventoryDto> CriticalStockItems { get; set; } = new();
        public List<InventoryDto> NearExpiryItems { get; set; } = new();
        public List<InventoryDto> ExpiredStockItems { get; set; } = new();
    }
}
