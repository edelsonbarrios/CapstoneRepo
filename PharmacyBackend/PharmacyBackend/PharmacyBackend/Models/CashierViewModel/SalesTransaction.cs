using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyBackend.Models.CashierViewModel
{
    public class SalesTransaction
    {
        [Key]
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }

        public int DiscountId { get; set; } // New

        [ForeignKey("DiscountId")]
        public Discount?
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            Discount { get; set; } // Navigation

        public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
    }
}
