//DTOs/DiscountDTOs/DiscountDto.cs
namespace PharmacyBackend.DTOs.DiscountDTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string DiscountName { get; set; }
        public string DiscountType {get; set;}
        public int DiscountAmount {get; set;}
        public DateTime? StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public bool IsActive { get; set; }
    }
}
