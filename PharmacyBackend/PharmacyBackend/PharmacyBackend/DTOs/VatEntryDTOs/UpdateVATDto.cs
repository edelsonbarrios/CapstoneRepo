namespace PharmacyBackend.DTOs.VatEntryDTOs
{
    public class UpdateVATDto : CreateVATDto
    {
        public bool IsActive { get; set; }
    }
}
