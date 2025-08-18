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
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        // Helper properties
        public bool IsExpired => DateTime.Now > ExpirationDate;
        public bool IsNearExpiry => DateTime.Now.AddDays(30) > ExpirationDate;
        public bool IsCriticalStock => QuantityOnHand <= ReorderLevel;
    }
}
