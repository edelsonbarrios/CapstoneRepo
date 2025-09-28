namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class StockAdjustmentDto
    {
        public int InventoryID { get; set; }
        public int BoxQuantityAdjustment { get; set; } = 0;
        public int SheetQuantityAdjustment { get; set; } = 0;
        public int PieceQuantityAdjustment { get; set; } = 0;
        public string MovementType { get; set; } = string.Empty; // "Damaged", "Expired", "Loss", "Found", etc.
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeID { get; set; }
    }
}
