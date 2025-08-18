using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class StockMovement
    {
        [Key]
        public int StockMovementID { get; set; }

        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; } // Navigation

        public string MovementType { get; set; } = string.Empty; // "Stock In", "Sale", "Adjustment", "Expired", "Damaged"

        public int QuantityChanged { get; set; } // Positive for stock in, negative for stock out

        public int QuantityBefore { get; set; }
        public int QuantityAfter { get; set; }

        public string? ReferenceNumber { get; set; } // Sales invoice, PO number, etc.

        public string? Notes { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.Now;

        public int? EmployeeID { get; set; } // Who made the movement
        public Employee? Employee { get; set; } // Navigation
    }
}
