using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Category
    {
        [Key] 
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
