namespace PharmacyBackend.DTOs.SupplierDTOs
{
    public class CreateSupplierDto
    {
        public string SupplierName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string ContactPerson { get; set;} = string.Empty;
        public string ContactPersonNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
