//Models/Brand.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }
}
