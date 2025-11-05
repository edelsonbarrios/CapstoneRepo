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

        public int? BrandID { get; set; }
        public Brand Brand { get; set; }  // Navigation

        public int? GenericID { get; set; }
        public Generic Generic { get; set; }  // Navigation

        public int? FormulationID { get; set; }
        public Formulation Formulation { get; set; }  // Navigation

        public int? ClassificationID { get; set; }
        public Classification Classification { get; set; }  // Navigation

        public int? TypeID { get; set; }
        public TypeModel TypeModel { get; set; }  // Navigation

        public List<Supplier> Suppliers { get; set; }
        public int SheetsPerBox { get; set; }
        public int PiecePerBox { get; set; }
        public int PiecesPerSheet { get; set; }
        public int CostPrice {get; set;}

        [Column(TypeName = "decimal(18,2)")]
        public decimal BoxPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SheetPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PiecePrice { get; set; }
        public int ReorderLevel { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public string UnitMeasurement { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItem { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<SalesItem> SalesItem { get; set; } = new List<SalesItem>();
        public ICollection<StockMovement> StockMovement { get; set; } = new List<StockMovement>();

    }
}
