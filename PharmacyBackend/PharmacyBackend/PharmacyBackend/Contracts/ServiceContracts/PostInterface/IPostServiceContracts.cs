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
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.Models.CashierViewModel;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        #region Employee
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        #endregion

        #region EmployeeHistory
        #endregion

        #region Account
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
        #endregion

        #region Sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto dto);
        public Task<SalesItemDto> CreateSalesItemAsync(CreateSalesItemDto dto);
        public Task<string> GenerateNewInvoiceNumberAsync();
        #endregion

        #region Discount
        public Task<bool> CreateDiscountAsync(CreateDiscountDto dto);
        public Task<bool> ValidateDiscountNameAsync(string name);
        #endregion

        #region Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        #endregion

        #region Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        public Task<bool> ValidateCategoryNameAsync(string name);
        #endregion

        #region Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);
        public Task<bool> ValidateBrandNameAsync(string name);

        #endregion

        #region Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);
        public Task<bool> ValidateGenericNameAsync(string name);
        #endregion

        #region Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);
        public Task<bool> ValidateFormulationNameAsync(string name);
        #endregion

        #region Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        public Task<bool> ValidateClassificationNameAsync(string name);
        #endregion

        #region Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);
        public Task<bool> ValidateTypeNameAsync(string name);
        #endregion

        #region Supplier
        public Task<bool> CreateSupplierAsync(CreateSupplierDto dto);
        #endregion

        #region Inventory
        public Task<bool> CreateInventoryAsync(CreateInventoryDto dto);

        public Task<bool> AdjustInventoryStockAsync(StockAdjustmentDto dto);
        #endregion

        #region VAT
        public Task<bool> CreateVATAsync(CreateVATDto dto);
        public Task<bool> VatNameValidationAsync(string name);
        #endregion

        #region PurchaseOrder
        public Task<bool> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
        public Task<string> GenerateNewPurchaseOrderNumberAsync();
        #endregion

        #region StockMovement
        public Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto dto);
        #endregion

        #region BackUp Database
        Task<CreateBackupResponse> CreateBackupAsync(CreateBackupRequest request);
        Task<RestoreBackupResponse> RestoreBackupAsync(RestoreBackupRequest request);
        Task<bool> UpdateBackupSettingsAsync(BackupSettingsDto settings);
        Task PerformAutomatedBackupAsync(); // For background service

        #endregion

    }
}
