using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;

namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class PurchaseOrderDTO
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<PurchaseOrderItemDto> PurchaseOrderItems { get; set; } = new();
    }
}
