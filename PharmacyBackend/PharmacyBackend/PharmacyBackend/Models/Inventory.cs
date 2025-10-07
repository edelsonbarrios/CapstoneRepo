using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; } // Navigation

        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;

        // Multi-unit quantities - maintain separate counts
        public int BoxQuantity { get; set; } = 0;
        public int SheetQuantity { get; set; } = 0; // Loose sheets
        public int PieceQuantity { get; set; } = 0; // Loose pieces

        // Unit specifications (optional - null if not applicable)
        public int? SheetsPerBox { get; set; }
        public int? PiecesPerSheet { get; set; }

        public int ReorderLevel { get; set; } // Still at box level

        [Column(TypeName = "decimal(18,2)")]
        public decimal BoxPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SheetPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PiecePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; } // Cost per box

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        // Helper properties
        public bool IsExpired => DateTime.UtcNow > ExpirationDate;
        public bool IsNearExpiry => DateTime.UtcNow.AddDays(30) > ExpirationDate;
        public bool IsCriticalStock => BoxQuantity <= ReorderLevel;

        // Calculate total pieces available
        public int TotalPiecesAvailable
        {
            get
            {
                var pieces = PieceQuantity;
                if (PiecesPerSheet.HasValue)
                    pieces += SheetQuantity * PiecesPerSheet.Value;
                if (SheetsPerBox.HasValue && PiecesPerSheet.HasValue)
                    pieces += BoxQuantity * SheetsPerBox.Value * PiecesPerSheet.Value;
                return pieces;
            }
        }

        // Check if product uses multi-unit system
        public bool IsMultiUnit => SheetsPerBox.HasValue && PiecesPerSheet.HasValue;
    }
}

