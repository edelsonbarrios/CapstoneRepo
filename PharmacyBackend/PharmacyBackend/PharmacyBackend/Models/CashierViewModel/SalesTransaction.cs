//Model/CashierViewMode/SalesTransaction.cs
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

        public int DiscountId { get; set; }

        [ForeignKey("DiscountId")]
        public Discount Discount { get; set; } // Navigation

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
    }
}
