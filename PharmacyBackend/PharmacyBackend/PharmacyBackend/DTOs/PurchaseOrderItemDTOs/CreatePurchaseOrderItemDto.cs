namespace PharmacyBackend.DTOs.PurchaseOrderItemDTOs
{
    public class CreatePurchaseOrderItemDto
    {
        public int ProductID { get; set; }
        public int BoxQuantityOrdered { get; set; }
        public int SheetQuantityOrdered { get; set; }
        public int PieceQuantityOrdered { get; set; }
        public decimal BoxPrice { get; set; }
        public decimal SheetPrice { get; set; }
        public decimal PiecePrice { get; set; }
        public string BatchNumber { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PoUnitType { get; set; }
    }
}
