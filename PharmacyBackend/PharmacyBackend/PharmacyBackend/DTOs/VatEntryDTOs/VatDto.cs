namespace PharmacyBackend.DTOs.VatEntryDTOs
{
    public class VatDto
    {
        public int VATID { get; set; }
        public string VATName { get; set; } = string.Empty;
        public decimal VATRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
