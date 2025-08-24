using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;

namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class CreatePurchaseOrderDto
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public int SupplierID { get; set; }
        public string? Notes { get; set; }
        public List<CreatePurchaseOrderItemDto> PurchaseOrderItems { get; set; } = new();
    }
}
