//DTOs//DiscountDTOs/UpdateDiscountDto.cs
namespace PharmacyBackend.DTOs.DiscountDTOs
{
    public class UpdateDiscountDto
    {
        public string DiscountName { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsActive { get; set; }
    }
}
