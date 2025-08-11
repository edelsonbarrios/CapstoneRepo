using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Classification
    {
        [Key]
        public int ClassificationID { get; set; }
        public string ClassificationName { get; set; } = string.Empty;
    }
}
