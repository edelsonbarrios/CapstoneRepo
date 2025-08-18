//DTOs/ProductDTOs/UpdateProductDto.cs
namespace PharmacyBackend.DTOs.ProductDTOs
{
    public class UpdateProductDto : CreateProductDto
    {
        public bool IsActive { get; set; }
    }
}


