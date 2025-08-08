using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public int GenericID { get; set; }
        public int FormulationID { get; set; }
        public int ClassificationID { get; set; }
        public int TypeID { get; set; }
        public int SupplierID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
