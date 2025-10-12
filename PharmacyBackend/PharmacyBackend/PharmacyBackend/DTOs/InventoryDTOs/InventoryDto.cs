using PharmacyBackend.DTOs.ProductDTOs;

namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class InventoryDto
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public ProductDto Product { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Multi-unit quantities
        public int BoxQuantity { get; set; }
        public int SheetQuantity { get; set; }
        public int PieceQuantity { get; set; }

        // Unit specifications
        public int? SheetsPerBox { get; set; }
        public int? PiecePerBox { get; set; }
        public int? PiecesPerSheet { get; set; }

        public int ReorderLevel { get; set; }
        public decimal CostPrice { get; set; }

        // Multi-level pricing
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }

        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public bool IsNearExpiry { get; set; }
        public bool IsCriticalStock { get; set; }
        public bool IsMultiUnit { get; set; }
        public int TotalPiecesAvailable { get; set; }
    }
}
