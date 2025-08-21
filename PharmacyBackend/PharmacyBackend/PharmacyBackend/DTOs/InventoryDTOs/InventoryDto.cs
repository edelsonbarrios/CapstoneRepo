namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class InventoryDto
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public decimal CostPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public bool IsNearExpiry { get; set; }
        public bool IsCriticalStock { get; set; }
    }
}
