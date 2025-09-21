//Model/CashierViewMode/SalesTransaction.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyBackend.Models.CashierViewModel
{
    public class SalesTransaction
    {
        [Key]
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VATAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        public string PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeAmount { get; set; }

        // Foreign Keys
        public int DiscountId { get; set; }
        public int EmployeeID { get; set; }
        public int? VATId { get; set; }

        // Navigation Properties
        [ForeignKey("DiscountId")]
        public Discount Discount { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }

        [ForeignKey("VATId")]
        public VAT VAT { get; set; }

        public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
    }
}
