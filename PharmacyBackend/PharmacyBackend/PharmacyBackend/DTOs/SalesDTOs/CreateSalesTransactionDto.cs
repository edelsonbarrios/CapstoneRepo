//DTOs/SalesDTOs/CreateSalesTransactionDto.cs
using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.DTOs.SalesDTOs
{
    public class CreateSalesTransactionDto
    {
        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ChangeAmount { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal VATAmount { get; set; } = 0;

        [Required]
        public int DiscountId { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        public int? VATId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateSalesItemDto> Items { get; set; } = new List<CreateSalesItemDto>();
        [Required]
        public decimal TotalAmount { get; set; }
    }
}
