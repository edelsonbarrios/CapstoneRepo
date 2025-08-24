namespace PharmacyBackend.DTOs.PurchaseOrderItemDTOs
{
    public class UpdatePurchaseOrderItemDto
    {
        public int PurchaseOrderItemID { get; set; }
        public int QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
    }
}
