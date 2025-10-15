using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactPersonNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Product> Products { get; set; } = new List<Product>();

        // Navigation properties
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
