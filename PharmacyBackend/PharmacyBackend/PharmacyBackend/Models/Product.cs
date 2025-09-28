//Model/Product.cs
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int CategoryID { get; set; }
        public Category Category { get; set; }  // Navigation

        public int BrandID { get; set; }
        public Brand Brand { get; set; }  // Navigation

        public int GenericID { get; set; }
        public Generic Generic { get; set; }  // Navigation

        public int FormulationID { get; set; }
        public Formulation Formulation { get; set; }  // Navigation

        public int ClassificationID { get; set; }
        public Classification Classification { get; set; }  // Navigation

        public int TypeID { get; set; }
        public TypeModel TypeModel { get; set; }  // Navigation

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }  // Navigation 

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItem { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<SalesItem> SalesItem { get; set; } = new List<SalesItem>();
        public ICollection<StockMovement> StockMovement { get; set; } = new List<StockMovement>();

    }
}
