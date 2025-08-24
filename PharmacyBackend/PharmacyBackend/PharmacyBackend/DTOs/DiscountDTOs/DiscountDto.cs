//DTOs/DiscountDTOs/DiscountDto.cs
namespace PharmacyBackend.DTOs.DiscountDTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
