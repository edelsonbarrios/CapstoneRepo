namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class PurchaseOrderReceivedRequest
    {
        public int PurchaseOrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public int EmployeeId { get; set; }
        public string ReceiptNumber { get; set; }
        public List<PurchaseOrderReceivedItemDTO> PurchaseOrderItems { get; set; }
    }
}
