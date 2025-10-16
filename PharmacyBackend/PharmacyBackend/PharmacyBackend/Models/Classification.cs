//Models/Classification.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Classification
    {
        [Key]
        public int ClassificationID { get; set; }
        public string ClassificationName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }
}
