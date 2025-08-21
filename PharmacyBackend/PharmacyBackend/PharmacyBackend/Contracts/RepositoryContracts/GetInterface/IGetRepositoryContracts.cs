using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.GetInterface
{
    public interface IGetRepositoryContracts
    {
        //employee
        #region Employees
        public Task<List<Employee>> GetAllEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<Account> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);
        public Task<List<Employee>> GetEmployeeByLastName(string lastName);
        #endregion

        //sales
        #region Sales
        public Task<List<SalesTransaction>> GetAllSalesTransactionsAsync();
        public Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id);
        #endregion
        public Task<SalesTransaction> GetSalesTransactionByIdAsync(int id);

        //Discount
        #region Discounts
        public Task<IEnumerable<Discount>> GetAllDiscountAsync();
        public Task<Discount> GetDiscountByIdAsync(int id);
        #endregion

        //Product
        #region Product
        public Task<IEnumerable<Product>> GetAllProductAsync();
        public Task<Product> GetProductByIdAsync(int id);
        #endregion

        //Category
        #region Category
        public Task<IEnumerable<Category>> GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        #endregion

        //Item Data Control

        //Brand
        #region Brand
        public Task<IEnumerable<Brand>> GetAllBrandAsync();
        public Task<Brand> GetBrandByIdAsync(int id);
        #endregion

        //Generic
        #region Generic
        public Task<IEnumerable<Generic>> GetAllGenericAsync();
        public Task<Generic> GetGenericByIdAsync(int id);
        #endregion

        //Formulation
        #region Formulation 
        public Task<IEnumerable<Formulation>> GetAllFormulationAsync();
        public Task<Formulation> GetFormulationByIdAsync(int id);
        #endregion

        //Classification
        #region Classification
        public Task<IEnumerable<Classification>> GetAllClassificationAsync();
        public Task<Classification> GetClassificationByIdAsync(int id);
        #endregion

        //Types
        #region Types
        public Task<IEnumerable<TypeModel>> GetAllTypeModelAsync();
        public Task<TypeModel> GetTypeModelByIdAsync(int id);
        #endregion

        #region Roles
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        #endregion

        //Supplier
        public Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        public Task<Supplier> GetSupplierByIdAsync(int id);

        //Inventory
        public Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        public Task<Inventory> GetInventoryByIdAsync(int id);
        public Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<Inventory>> GetExpiredInventoryAsync();
        public Task<IEnumerable<Inventory>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync();

        //VAT
        public Task<IEnumerable<VAT>> GetAllVATAsync();
        public Task<VAT> GetVATByIdAsync(int id);

        //PurchaseOrder
        public Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();
        public Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
        public Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderBySupplierIdAsync(int supplierId);
        public Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderByStatusAsync(string status);

        //StockMovement
        public Task<IEnumerable<StockMovement>> GetAllStockMovementAsync();
        public Task<StockMovement> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovement>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovement>> GetStockMovementByProductIdAsync(int productId);
    }
}
