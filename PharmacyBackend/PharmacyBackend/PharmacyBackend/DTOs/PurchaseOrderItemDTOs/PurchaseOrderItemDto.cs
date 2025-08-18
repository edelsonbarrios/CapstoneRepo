namespace PharmacyBackend.DTOs.PurchaseOrderItemDTOs
{
    public class PurchaseOrderItemDto
    {
        public int PurchaseOrderItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityOrdered { get; set; }
        public int QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
