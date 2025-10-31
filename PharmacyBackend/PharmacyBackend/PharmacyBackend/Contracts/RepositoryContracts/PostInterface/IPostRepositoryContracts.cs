using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.PostInterface
{
    public interface IPostRepositoryContracts
    {
        #region Employee
        public Task<int> CreateEmployeeAsync(Employee request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        #endregion

        #region EmployeeHistory
        public Task<bool> CreateEmployeeHistoryAsync(EmployeeHistory request);
        #endregion

        #region Account
        public Task<bool> CreateAccountAsync(Account request);
        #endregion

        #region Sales

        public Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction salesTransaction);
        public Task<SalesItem> CreateSalesItemAsync(SalesItem salesItem);
        public Task<string> GenerateNewInvoiceNumberAsync();
      

        #endregion

        #region Discount
        public Task<bool> CreateDiscountAsync(Discount discount);
        public Task<bool> ValidateDiscountNameAsync(string name);
        #endregion

        #region Product
        public Task<Product> CreateProductAsync(Product product);
        #endregion

        #region Category
        public Task<Category> CreateCategoryAsync(Category category);
        public Task<bool> ValidateCategoryNameAsync(string name);
        #endregion

        #region Brand
        public Task<Brand> CreateBrandAsync(Brand brand);
        public Task<bool> ValidateBrandNameAsync(string name);
        #endregion

        #region Generic 
        public Task<Generic> CreateGenericAsync(Generic generic);
        public Task<bool> ValidateGenericNameAsync(string name);
        #endregion

        #region Formulation
        public Task<Formulation> CreateFormulationAsync(Formulation formulation);
        public Task<bool> ValidateFormulationNameAsync(string name);
        #endregion

        #region Classification
        public Task<Classification> CreateClassificationAsync(Classification classification);
        public Task<bool> ValidateClassificationNameAsync(string name);
        #endregion

        #region Types
        public Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel);
        public Task<bool> ValidateTypeNameAsync(string name);
        #endregion

        #region Supplier
        public Task<bool> CreateSupplierAsync(Supplier supplier);
        #endregion

        #region Inventory
        public Task<bool> CreateInventoryAsync(Inventory inventory);
        #endregion

        #region VAT
        public Task<bool> CreateVATAsync(VAT vat);
        public Task<bool> VatNameValidationAsync(string name);
        #endregion

        #region PurchaseOrder
        public Task<bool> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        public Task<string> GenerateNewPurchaseOrderNumberAsync();
        #endregion

        #region PurchaserOrderItem
        public Task<bool> CreatePurchaseOrderItemsAsync(List<PurchaseOrderItem> request);
        #endregion

        #region StockMovement
        public Task<StockMovement> CreateStockMovementAsync(StockMovement stockMovement);
        #endregion

        #region BackUp Database

        Task<CreateBackupResponse> CreateBackupAsync(CreateBackupRequest request);
        Task<RestoreBackupResponse> RestoreBackupAsync(RestoreBackupRequest request);

        #endregion
    }
}

