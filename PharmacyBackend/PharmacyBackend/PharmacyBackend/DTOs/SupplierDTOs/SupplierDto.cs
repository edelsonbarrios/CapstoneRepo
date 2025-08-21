namespace PharmacyBackend.DTOs.SupplierDTOs
{
    public class SupplierDto
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
