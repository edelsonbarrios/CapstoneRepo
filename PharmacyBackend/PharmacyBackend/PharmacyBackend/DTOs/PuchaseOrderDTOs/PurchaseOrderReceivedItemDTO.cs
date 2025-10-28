namespace PharmacyBackend.DTOs.PuchaseOrderDTOs
{
    public class PurchaseOrderReceivedItemDTO
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int BoxQuantityReceived { get; set; }
        public int SheetQuantityReceived { get; set; }
        public int PieceQuantityReceived { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DamagedPiecesCount {get; set; }
    }
}
