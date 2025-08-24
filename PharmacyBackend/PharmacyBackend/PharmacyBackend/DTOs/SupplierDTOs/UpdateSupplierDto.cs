namespace PharmacyBackend.DTOs.SupplierDTOs
{
    public class UpdateSupplierDto : CreateSupplierDto
    {
        public bool IsActive { get; set; }
    }
}
