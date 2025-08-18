namespace PharmacyBackend.DTOs.PurchaseOrderItemDTOs
{
    public class CreatePurchaseOrderItemDto
    {
        public int ProductID { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal UnitCost { get; set; }
    }
}
