using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using System.Threading.Tasks;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);

        //sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);

        //Discount
        public Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);

        //Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);

        //Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);

        //ItemDataControl

        //Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);

        //Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);

        //Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);

        //Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        //Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);

        //Supplier
        public Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);

        //Inventory
        public Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto dto);

        //VAT
        public Task<VatDto> CreateVATAsync(CreateVATDto dto);

        //PurchaseOrder
        public Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);

        //StockMovement
        public Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto dto);


    }
}
