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

        //sales
        #region Sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);
        #endregion

        //Discount
        #region Discount
        public Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);
        #endregion

        //Product
        #region Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        #endregion

        //Category
        #region Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        #endregion

        //ItemDataControl

        //Brand
        #region Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);
        #endregion

        //Generic
        #region Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);
        #endregion

        //Formulation
        #region Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);
        #endregion

        //Classification
        #region Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        #endregion
        //Type
        #region Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);
        #endregion

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
