namespace PharmacyBackend.DTOs.ReportsDTOs
{
    public class FastMovingProductDataDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int QuantitySold { get; set; }
        public int TransactionCount { get; set; }
        public decimal Revenue { get; set; }
        public int CurrentStock { get; set; }
    }
}
