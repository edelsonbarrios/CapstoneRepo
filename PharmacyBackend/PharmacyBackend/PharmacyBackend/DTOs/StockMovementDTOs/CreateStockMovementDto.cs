namespace PharmacyBackend.DTOs.StockMovementDTOs
{
    public class CreateStockMovementDto
    {
        public int InventoryID { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public int QuantityChanged { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeID { get; set; }
    }
}
