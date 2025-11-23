//Models/Discount.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string DiscountName { get; set; } = string.Empty;
        public string DiscountType {get; set;} 
        public int DiscountAmount {get; set;}
        public DateTime? StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public List<Inventory> Inventories {get; set;}

    }
}
