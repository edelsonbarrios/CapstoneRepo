using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.PostInterface
{
    public interface IPostRepositoryContracts
    {
        public Task<int> CreateEmployeeAsync(Employee request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(Account request);

        //sales
        public Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction transaction);

        //Discount
        public Task<Discount> CreateDiscountAsync(Discount discount);

        //Product
        public Task<Product> CreateProductAsync(Product product);

        //Category
        public Task<Category> CreateCategoryAsync(Category category);

        //ItemDataControl

        //Brand
        public Task<Brand> CreateBrandAsync(Brand brand);

        //Generic
        public Task<Generic> CreateGenericAsync(Generic generic);

        //Formulation
        public Task<Formulation> CreateFormulationAsync(Formulation formulation);

        //Classification
        public Task<Classification> CreateClassificationAsync(Classification classification);

        //Types
        public Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel);

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

