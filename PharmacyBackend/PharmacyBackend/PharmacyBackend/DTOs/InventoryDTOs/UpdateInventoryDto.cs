namespace PharmacyBackend.DTOs.InventoryDTOs
{
    public class UpdateInventoryDto : CreateInventoryDto
    {
        public bool IsActive { get; set; }
    }

}
