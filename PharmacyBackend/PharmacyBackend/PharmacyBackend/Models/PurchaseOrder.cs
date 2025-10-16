using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public int SupplierID { get; set; }
        public int EmployeeID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Received, 
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Supplier Supplier { get; set; }
        public Employee Employee { get; set; }
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
