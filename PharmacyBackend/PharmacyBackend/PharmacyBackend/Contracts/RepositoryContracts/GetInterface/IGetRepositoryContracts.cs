using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.GetInterface
{
    public interface IGetRepositoryContracts
    {

        #region Employees
        public Task<List<Employee>> GetAllEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);
        public Task<List<Employee>> GetEmployeeByLastName(string lastName);
        public Task<List<Employee>> GetFirstDisplayEmployees();
        #endregion

        #region Account
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<Account> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);
        public Task<Account> SearchAccountByUsernameAsync(string userName);
        #endregion

        #region Sales
        public Task<List<SalesTransaction>> GetAllSalesTransactionsAsync();
        public Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id);
        #endregion

        #region Discounts
        public Task<IEnumerable<Discount>> GetAllDiscountAsync();
        public Task<Discount> GetDiscountByIdAsync(int id);
        #endregion

        #region Product
        public Task<IEnumerable<Product>> GetAllProductAsync();
        public Task<Product> GetProductByIdAsync(int id);
        #endregion

        #region Category
        public Task<IEnumerable<Category>> GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        #endregion

        #region Brand
        public Task<IEnumerable<Brand>> GetAllBrandAsync();
        public Task<Brand> GetBrandByIdAsync(int id);
        #endregion

        #region Generic
        public Task<IEnumerable<Generic>> GetAllGenericAsync();
        public Task<Generic> GetGenericByIdAsync(int id);
        #endregion

        #region Formulation 
        public Task<IEnumerable<Formulation>> GetAllFormulationAsync();
        public Task<Formulation> GetFormulationByIdAsync(int id);
        #endregion

        #region Classification
        public Task<IEnumerable<Classification>> GetAllClassificationAsync();
        public Task<Classification> GetClassificationByIdAsync(int id);
        #endregion

        #region Types
        public Task<IEnumerable<TypeModel>> GetAllTypeModelAsync();
        public Task<TypeModel> GetTypeModelByIdAsync(int id);
        #endregion

        #region Roles
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        #endregion

        #region Supplier
        public Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        public Task<Supplier> GetSupplierByIdAsync(int id);
        #endregion

        #region Inventory
        public Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        public Task<Inventory> GetInventoryByIdAsync(int id);
        public Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<Inventory>> GetExpiredInventoryAsync();
        public Task<IEnumerable<Inventory>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync();
        #endregion

        #region VAT
        public Task<IEnumerable<VAT>> GetAllVATAsync();
        public Task<VAT> GetVATByIdAsync(int id);
        #endregion

        #region PurchaseOrder
        public Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();
        public Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
        public Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderBySupplierIdAsync(int supplierId);
        public Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderByStatusAsync(string status);
        #endregion

        #region StockMovement
        public Task<IEnumerable<StockMovement>> GetAllStockMovementAsync();
        public Task<StockMovement> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovement>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovement>> GetStockMovementByProductIdAsync(int productId);
        #endregion
    }
}
