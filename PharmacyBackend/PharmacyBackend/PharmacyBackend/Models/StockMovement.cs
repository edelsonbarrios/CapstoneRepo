using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class StockMovement
    {
        [Key]
        public int StockMovementID { get; set; }

        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; } // Navigation

        public string MovementType { get; set; } = string.Empty; // "Stock In", "Sale", "Adjustment", "Expired", "Damaged", "Count Correction"

        // Multi-unit changes
        public int BoxQuantityChanged { get; set; } = 0;
        public int SheetQuantityChanged { get; set; } = 0;
        public int PieceQuantityChanged { get; set; } = 0;

        // Before quantities
        public int BoxQuantityBefore { get; set; } = 0;
        public int SheetQuantityBefore { get; set; } = 0;
        public int PieceQuantityBefore { get; set; } = 0;

        // After quantities
        public int BoxQuantityAfter { get; set; } = 0;
        public int SheetQuantityAfter { get; set; } = 0;
        public int PieceQuantityAfter { get; set; } = 0;

        // Backward compatibility
        public int QuantityChanged => BoxQuantityChanged; // For existing code
        public int QuantityBefore => BoxQuantityBefore; // For existing code
        public int QuantityAfter => BoxQuantityAfter; // For existing code

        public string? ReferenceNumber { get; set; } // Sales invoice, PO number, etc.
        public string? Notes { get; set; }
        public DateTime MovementDate { get; set; } = DateTime.UtcNow.AddHours(8);
        public int? EmployeeID { get; set; } // Who made the movement
        public Employee? Employee { get; set; } // Navigation
    }
}
