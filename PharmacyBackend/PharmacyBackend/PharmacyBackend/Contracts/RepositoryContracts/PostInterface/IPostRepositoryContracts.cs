using PharmacyBackend.DTOs.AccountDTOs;
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
      

        #endregion

        #region Discount
        public Task<Discount> CreateDiscountAsync(Discount discount);
        #endregion

        #region Product
        public Task<Product> CreateProductAsync(Product product);
        #endregion

        #region Category
        public Task<Category> CreateCategoryAsync(Category category);
        #endregion

        #region Brand
        public Task<Brand> CreateBrandAsync(Brand brand);
        #endregion

        #region Generic 
        public Task<Generic> CreateGenericAsync(Generic generic);
        #endregion

        #region Formulation
        public Task<Formulation> CreateFormulationAsync(Formulation formulation);
        #endregion

        #region Classification
        public Task<Classification> CreateClassificationAsync(Classification classification);
        #endregion

        #region Types
        public Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel);
        #endregion

        #region Supplier
        public Task<bool> CreateSupplierAsync(Supplier supplier);
        #endregion

        #region Inventory
        public Task<bool> CreateInventoryAsync(Inventory inventory);
        #endregion

        #region VAT
        public Task<VAT> CreateVATAsync(VAT vat);
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
    }
}

