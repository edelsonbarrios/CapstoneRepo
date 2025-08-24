using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;

namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class UpdatePurchaseOrderDto
    {
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<UpdatePurchaseOrderItemDto> PurchaseOrderItems { get; set; } = new();
}
}
