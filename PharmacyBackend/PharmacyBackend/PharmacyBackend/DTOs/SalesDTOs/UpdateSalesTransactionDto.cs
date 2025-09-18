//DTOs/SalesDTOs/UpdateSalesTransactionDto.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class UpdateSalesTransactionDto
    {
        [Required]
        public int Id { get; set; }

        public DateTime? TransactionDate { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TotalAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DiscountAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? VATAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? NetAmount { get; set; }

        [StringLength(20)]
        public string PaymentMethod { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? AmountPaid { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ChangeAmount { get; set; }

        public int? DiscountId { get; set; }
        public int? EmployeeID { get; set; }
        public int? VATId { get; set; }

        public List<UpdateSalesItemDto> Items { get; set; } = new List<UpdateSalesItemDto>();
    }
}