//Model/CashierViewModel/SalesItem.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyBackend.Models.CashierViewModel
{
    public class SalesItem
    {
        [Key]
        public int Id { get; set; }

        public int SalesTransactionId { get; set; }
        public SalesTransaction SalesTransaction { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
