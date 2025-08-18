//Models/Generic.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Generic
    {
        [Key]
        public int GenericID { get; set; }
        public string GenericName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; }
    }
}
