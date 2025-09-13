using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int PurchaseOrderID { get; set; }
        public int ProductID { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityReceived { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        //Navigation Properties
        public Product Product { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
