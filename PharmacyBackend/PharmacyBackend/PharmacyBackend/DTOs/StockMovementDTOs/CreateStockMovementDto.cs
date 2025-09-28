namespace PharmacyBackend.DTOs.StockMovementDTOs
{
    public class CreateStockMovementDto
    {
        public int InventoryID { get; set; }
        public string MovementType { get; set; } = string.Empty; // "Damaged", "Expired", "Loss", etc

        // Multi-unit changes
        public int BoxQuantityChanged { get; set; } = 0;
        public int SheetQuantityChanged { get; set; } = 0;
        public int PieceQuantityChanged { get; set; } = 0;

        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeID { get; set; }
    }
}
