//DTOs/Product.cs
namespace PharmacyBackend.DTOs.ProductDTOs
{
    public class ProductDto : UpdateProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public int? BrandID { get; set; }
        public int? GenericID { get; set; }
        public int? FormulationID { get; set;}
        public int? ClassificationID { get; set; }
        public int? TypeID { get; set; }
        public int? SupplierID { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int? SheetsPerBox { get; set; }
        public int? PiecesPerSheet { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }
        public int ReorderLevel { get; set; }
        public string UnitMeasurement { get; set; }

        public string Description { get; set; } = string.Empty;
        public int? PiecePerBox { get; set; }
        // Calculated fields from Inventory
        public int TotalStock { get; set; } // Calculated from active, non-expired inventory
        public int ExpiredStock { get; set; } // Expired inventory count
        public int NearExpiryStock { get; set; } // Stock expiring within 30 days
        public bool IsCriticalStock { get; set; } // If total stock is below reorder level
        public bool HasExpiredStock { get; set; } // If there's any expired stock

        // Related entity names for display
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string FormulationName { get; set; } = string.Empty;
        public string ClassificationName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
    }
}
