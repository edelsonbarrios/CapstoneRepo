namespace PharmacyBackend.DTOs.StockMovementDTOs
{
    public class StockMovementDto
    {
        public int StockMovementID { get; set; }
        public int InventoryID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;

        // Multi-unit changes
        public int BoxQuantityChanged { get; set; }
        public int SheetQuantityChanged { get; set; }
        public int PieceQuantityChanged { get; set; }

        // Before quantities
        public int BoxQuantityBefore { get; set; }
        public int SheetQuantityBefore { get; set; }
        public int PieceQuantityBefore { get; set; }

        // After quantities
        public int BoxQuantityAfter { get; set; }
        public int SheetQuantityAfter { get; set; }
        public int PieceQuantityAfter { get; set; }

        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime MovementDate { get; set; }
        public int? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
    }
}
