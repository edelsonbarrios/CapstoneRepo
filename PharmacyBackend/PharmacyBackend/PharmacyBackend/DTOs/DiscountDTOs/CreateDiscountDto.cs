//DTOs/DiscountDTOs/CreateDiscountDto.cs
namespace PharmacyBackend.DTOs.DiscountDTOs
{
    public class 
      CreateDiscountDto
    {
        public string DiscountName { get; set; }
        public string DiscountType {get; set;}
        public int DiscountAmount {get; set;}
        public DateTime? StartDate {get; set;}
        public DateTime? EndDate {get; set;}
    }
}
