namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class CreateInventoryDto
    {
        public string ProductName { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Multi-unit quantities
        public int BoxQuantity { get; set; }
        public int SheetQuantity { get; set; } = 0;
        public int PieceQuantity { get; set; } = 0;

        // Unit specifications (optional)
        public int? SheetsPerBox { get; set; }
        public int? PiecesPerSheet { get; set; }

        public int ReorderLevel { get; set; }
        public decimal CostPrice { get; set; }

        // Multi-level pricing
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }

        //for stock movement
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeID { get; set; }
    }
}
