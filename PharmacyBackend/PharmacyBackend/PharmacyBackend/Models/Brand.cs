using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }
        public string BrandName { get; set; } = string.Empty;

    }
}
