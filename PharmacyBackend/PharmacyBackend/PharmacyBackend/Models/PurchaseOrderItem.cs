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
        public int BoxQuantityOrdered { get; set; }
        public int BoxQuantityReceived { get; set; } = 0;
        public int SheetQuantityOrdered { get; set; }
        public int SheetQuantityReceived { get; set; }
        public int PieceQuantityOrdered { get; set; }
        public int PieceQuantityReceived { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BoxPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SheetPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PiecePrice { get; set; }
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
