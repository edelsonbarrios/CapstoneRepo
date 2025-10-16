//Models/TypeModel.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class TypeModel
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }
}
