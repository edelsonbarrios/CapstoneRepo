namespace PharmacyBackend.DTOs.VatEntryDTOs
{
    public class CreateVATDto
    {
        public string VATName { get; set; } = string.Empty;
        public decimal VATRate { get; set; }
    }
}
