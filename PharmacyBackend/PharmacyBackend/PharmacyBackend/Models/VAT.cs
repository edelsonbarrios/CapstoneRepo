using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyBackend.Models
{
    public class VAT
    {
        [Key]
        public int VATID { get; set; }

        public string VATName { get; set; } = string.Empty; // e.g., "Standard VAT", "Zero VAT"

        [Column(TypeName = "decimal(5,2)")]
        public decimal VATRate { get; set; } // e.g., 12.00 for 12%

        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
