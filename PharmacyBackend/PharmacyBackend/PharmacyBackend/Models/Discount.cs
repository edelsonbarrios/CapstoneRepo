//Models/Discount.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string DiscountName { get; set; } = string.Empty;
        public decimal DiscountRate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
