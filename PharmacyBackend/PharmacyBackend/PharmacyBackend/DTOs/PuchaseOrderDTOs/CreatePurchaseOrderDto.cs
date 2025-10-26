using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;

namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class CreatePurchaseOrderDto
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public string EmployeeLastName { get; set; }
        public string SupplierName { get; set; }
        public string? Notes { get; set; }
        public string ReceiptNumber { get; set; }
        public List<CreatePurchaseOrderItemDto> PurchaseOrderItems { get; set; } = new();
    }
}
