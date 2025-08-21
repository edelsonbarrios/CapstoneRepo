using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
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

        #region Account
        public Task<bool> CreateAccountAsync(Account request);
        #endregion

        //sales
        #region --Sales
        public Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction transaction);
        #endregion

        //Discount
        #region Discount
        public Task<Discount> CreateDiscountAsync(Discount discount);
        #endregion

        //Product
        #region Product
        public Task<Product> CreateProductAsync(Product product);
        #endregion

        //Category
        #region Category
        public Task<Category> CreateCategoryAsync(Category category);
        #endregion

        //ItemDataControl

        //Brand
        #region Brand
        public Task<Brand> CreateBrandAsync(Brand brand);
        #endregion

        //Generic
        #region Generic 
        public Task<Generic> CreateGenericAsync(Generic generic);
        #endregion

        //Formulation
        #region Formulation
        public Task<Formulation> CreateFormulationAsync(Formulation formulation);
        #endregion

        //Classification
        #region Classification
        public Task<Classification> CreateClassificationAsync(Classification classification);
        #endregion

        //Types
        #region Types
        public Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel);
        #endregion

        //Supplier
        public Task<Supplier> CreateSupplierAsync(Supplier supplier);

        //Inventory
        public Task<Inventory> CreateInventoryAsync(Inventory inventory);

        //VAT
        public Task<VAT> CreateVATAsync(VAT vat);

        //PurchaseOrder
        public Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);

        //StockMovement
        public Task<StockMovement> CreateStockMovementAsync(StockMovement stockMovement);
    }
}

