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
        #region Employee
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        #endregion

        #region Account
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
        #endregion

        #region Sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);
        #endregion

        #region Discount
        public Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);
        #endregion

        #region Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        #endregion

        #region Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        #endregion

        #region Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);
        #endregion

        #region Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);
        #endregion

        #region Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);
        #endregion

        #region Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        #endregion

        #region Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);
        #endregion

        #region Supplier
        public Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        #endregion

        #region Inventory
        public Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto dto);
        #endregion

        #region VAT
        public Task<VatDto> CreateVATAsync(CreateVATDto dto);
        #endregion

        #region PurchaseOrder
        public Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
        #endregion

        #region StockMovement
        public Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto dto);
        #endregion

    }
}
