using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderID { get; set; }

        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; } // Navigation

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Received, Cancelled

        public string? Notes { get; set; }

        // Navigation property
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
