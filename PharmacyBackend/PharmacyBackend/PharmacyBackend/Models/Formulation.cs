//Models/Formulation.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Formulation
    {
        [Key]
        public int FormulationID { get; set; }
        public string FormulationName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }
}
