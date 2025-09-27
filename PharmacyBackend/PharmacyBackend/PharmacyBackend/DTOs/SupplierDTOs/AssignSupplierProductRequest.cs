using PharmacyBackend.DTOs.ProductDTOs;

namespace PharmacyBackend.DTOs.SupplierDTOs
{
    public class AssignSupplierProductRequest
    {
        public int SupplierId { get; set; }
        public List<ProductDto> ProductsToBeAdded { get; set; }
    }
}
