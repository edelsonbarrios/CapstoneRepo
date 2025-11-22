using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.ReportsDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.StockHistoryDTOs;
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

        #region EmployeeHistory 
        public Task<List<EmployeeHistory>> GetAllEmployeeHistoryAsync();
        public Task<EmployeeHistory> GetEmployeeHistoryByIdAsync(int employeeHistoryId);
        public Task<List<EmployeeHistory>> GetDisplayEmployeeHistoryAsync();
        #endregion

        #region Account
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<Account> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);
        public Task<Account> SearchAccountByUsernameAsync(string userName);
        public Task<Account> GetAccountByEmployeeIdAsync(int employeeId);
        #endregion

        #region Daily Sales
        public Task<DailySalesSummaryDto> GetSummarySalesForToday();
        Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId = null,
            string paymentMethod = null
        );

        Task<IEnumerable<ProductSalesSummaryDto>> GetProductSalesSummaryAsync(
            DateTime startDate,
            DateTime endDate
        );

        Task<IEnumerable<SalesTransaction>> GetFilteredSalesTransactionsAsync(
            DateTime startDate,
            DateTime endDate,
            int? employeeId = null,
            string paymentMethod = null
        );
        #endregion

        #region Sales
        // Sales Transaction methods
        public Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id);
        public Task<IEnumerable<SalesTransaction>> GetAllSalesTransactionsAsync();
        public Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByEmployeeIdAsync(int employeeId);
        public Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber);

        // Sales Item methods
        public Task<SalesItem?> GetSalesItemByIdAsync(int id);
        public Task<IEnumerable<SalesItem>> GetSalesItemsByTransactionIdAsync(int transactionId);
        public Task<bool> ReferenceNumberValidationAsync(string refNum);


        #endregion

        #region Discounts
        public Task<IEnumerable<Discount>> GetAllDiscountAsync();
        public Task<Discount> GetDiscountByIdAsync(int id);
        public Task<List<Discount>> GetAllAvailableDiscountsAsync();
        #endregion

        #region Product
        public Task<IEnumerable<Product>> GetAllProductAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<List<Product>> GetAllInactiveProductsAsync();
        public Task<Product> GetProductByNameAsync(string name);
        #endregion

        #region Category
        public Task<IEnumerable<Category>> GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<List<Category>> GetAllInactiveCategoriesAsync();
        #endregion

        #region Brand
        public Task<IEnumerable<Brand>> GetAllBrandAsync();
        public Task<Brand> GetBrandByIdAsync(int id);
        public Task<List<Brand>> GetAllInactiveBrandsAsync();
        #endregion

        #region Generic
        public Task<IEnumerable<Generic>> GetAllGenericAsync();
        public Task<Generic> GetGenericByIdAsync(int id);
        public Task<List<Generic>> GetAllInactiveGenericsAsync();
        #endregion

        #region Formulation 
        public Task<IEnumerable<Formulation>> GetAllFormulationAsync();
        public Task<Formulation> GetFormulationByIdAsync(int id);
        public Task<List<Formulation>> GetAllInactiveFormulationsAsync();
        #endregion

        #region Classification
        public Task<IEnumerable<Classification>> GetAllClassificationAsync();
        public Task<Classification> GetClassificationByIdAsync(int id);
        public Task<List<Classification>> GetAllInactiveClassificationsAsync();
        #endregion

        #region Types
        public Task<IEnumerable<TypeModel>> GetAllTypeModelAsync();
        public Task<TypeModel> GetTypeModelByIdAsync(int id);
        public Task<List<TypeModel>> GetAllInactiveTypesAsync();
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
        public Task<List<Inventory>> GetAllAvailableInventoryAsync();
        public Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<Inventory>> GetExpiredInventoryAsync();
        public Task<IEnumerable<Inventory>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync();
        public Task<List<Inventory>> GetInventoriesOutOfStockAsync();
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
        public Task<List<PurchaseOrder>> GetPendingPurchaseOrdersAsync();
        #endregion

        #region StockMovement
        public Task<IEnumerable<StockMovement>> GetAllStockMovementAsync();
        public Task<StockMovement> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovement>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovement>> GetStockMovementByProductIdAsync(int productId);
        #endregion

        #region Reports
        // Sales Reports
        Task<IEnumerable<SalesTransaction>> GetSalesReportDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId,
            string paymentMethod);

        Task<IEnumerable<Inventory>> GetInventoryReportDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            string filterType);

        Task<IEnumerable<FastMovingProductDataDto>> GetFastMovingProductsDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            int topN);
        #endregion

        #region AdminDashboard
        public Task<decimal> GetTodaySalesAmountAsync();
        public Task<int> GetInventoryLowStockCountAsync();
        public Task<int> GetInventoryOutOfStockCountAsync();
        public Task<List<ProductSalesSummaryDto>> GetTopSellingProductsMonthlyAsync();
        #endregion
        
        #region BackUp Database 
        Task<BackupListResponse> GetAllBackupsAsync();
        Task<byte[]?> DownloadBackupAsync(string fileName);
        string GetBackupDirectory();
        #endregion

        #region StockHistory

        Task<IEnumerable<StockMovement>> GetStockHistoryAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? searchTerm);

        Task<StockHistorySummaryDto> GetStockHistorySummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType);

        #endregion
    }
}
