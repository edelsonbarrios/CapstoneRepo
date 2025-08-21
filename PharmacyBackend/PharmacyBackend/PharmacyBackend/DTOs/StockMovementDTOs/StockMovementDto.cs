namespace PharmacyBackend.DTOs.StockMovementDTOs
{
    public class StockMovementDto
    {
        public int StockMovementID { get; set; }
        public int InventoryID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public int QuantityChanged { get; set; }
        public int QuantityBefore { get; set; }
        public int QuantityAfter { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime MovementDate { get; set; }
        public int? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
    }
}
