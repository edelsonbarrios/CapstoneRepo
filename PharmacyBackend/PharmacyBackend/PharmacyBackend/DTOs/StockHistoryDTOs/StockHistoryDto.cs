namespace PharmacyBackend.DTOs.StockHistoryDTOs
{
    public class StockHistoryDto
    {
        public int StockMovementID { get; set; }
        public DateTime MovementDate { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; }

        // Before quantities
        public int BoxQuantityBefore { get; set; }
        public int SheetQuantityBefore { get; set; }
        public int PieceQuantityBefore { get; set; }

        // Changed quantities
        public int BoxQuantityChanged { get; set; }
        public int SheetQuantityChanged { get; set; }
        public int PieceQuantityChanged { get; set; }

        // After quantities
        public int BoxQuantityAfter { get; set; }
        public int SheetQuantityAfter { get; set; }
        public int PieceQuantityAfter { get; set; }

        public string PerformedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
