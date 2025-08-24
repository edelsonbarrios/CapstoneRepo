namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class CreateInventoryDto
    {
        public int ProductID { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public decimal CostPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
