namespace PharmacyBackend.DTOs.PurchaseOrderItemDTOs
{
    public class PurchaseOrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int BoxQuantityOrdered { get; set; }
        public int BoxQuantityReceived { get; set; } = 0;
        public int SheetQuantityOrdered { get; set; }
        public int SheetQuantityReceived { get; set; }
        public int PieceQuantityOrdered { get; set; }
        public int PieceQuantityReceived { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }
        public decimal TotalCost { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
