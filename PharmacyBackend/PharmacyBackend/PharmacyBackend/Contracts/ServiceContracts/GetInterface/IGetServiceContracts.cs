using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using System.Collections;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.Models.CashierViewModel;
using PharmacyBackend.DTOs.DashboardDTOs;
using PharmacyBackend.DTOs.DailySaleDTOs;
using System.Runtime.CompilerServices;
using PharmacyBackend.DTOs.ReportsDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.StockHistoryDTOs;
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
        #region DashBoardInventoryMan
        public Task<InventoryDashboardDto> GetInventoryDashboardAsync();

        #endregion


        #region Employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        public Task<List<EmployeeDTO>> GetEmployeeByLastName(string lastName);
        public Task<List<EmployeeDTO>> GetFirstDisplayEmployees();
        #endregion

        #region EmployeeHistory 
        public Task<List<EmployeeHistoryDTO>> GetAllEmployeeHistoryAsync();
        public Task<EmployeeHistoryDTO> GetEmployeeHistoryByIdAsync(int employeeHistoryId);
        public Task<List<EmployeeHistoryDTO>> GetDisplayEmployeeHistoryAsync();
        #endregion

        #region Accounts
        public Task<AccountDTO> SearchAccountByUsernameAsync(string userName);
        public Task<List<AccountDTO>> GetAllAccountsAsync();
        public Task<AccountDTO> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);
        public Task<AccountDTO> GetAccountByEmployeeIdAsync(int employeeId);
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

        Task<IEnumerable<SalesTransactionDto>> GetFilteredSalesTransactionsAsync(
            DateTime startDate,
            DateTime endDate,
            int? employeeId = null,
            string paymentMethod = null
        );
        #endregion

        #region Sales
        // Sales Transaction methods
        public Task<SalesTransactionDto?> GetSalesTransactionByIdAsync(int id);
        public Task<IEnumerable<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByEmployeeIdAsync(int employeeId);
        public Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber);
        public Task<SalesItemDto?> GetSalesItemByIdAsync(int id);
        public Task<IEnumerable<SalesItemDto>> GetSalesItemsByTransactionIdAsync(int transactionId);
        public Task<bool> ReferenceNumberValidationAsync(string refNum);
        #endregion

        #region Discount
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);
        #endregion

        #region Product
        public Task<IEnumerable<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        public Task<List<ProductDto>> GetAllInactiveProductsAsync();
        public Task<ProductDto> GetProductByNameAsync(string name);
        #endregion

        #region Category
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);
        public Task<List<CategoryDto>> GetAllInactiveCategoriesAsync();
        #endregion

        #region Brand
        public Task<IEnumerable<BrandDto>> GetAllBrandAsync();
        public Task<BrandDto> GetBrandByIdAsync(int id);
        public Task<List<BrandDto>> GetAllInactiveBrandsAsync();
        #endregion

        #region Generic
        public Task<IEnumerable<GenericDto>> GetAllGenericAsync();
        public Task<GenericDto> GetGenericByIdAsync(int id);
        public Task<List<GenericDto>> GetAllInactiveGenericsAsync();
        #endregion

        #region Formulation
        public Task<IEnumerable<FormulationDto>> GetAllFormulationAsync();
        public Task<FormulationDto> GetFormulationByIdAsync(int id);
        public Task<List<FormulationDto>> GetAllInactiveFormulationsAsync();
        #endregion

        #region Classification
        public Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync();
        public Task<ClassificationDto> GetClassificationByIdAsync(int id);
        public Task<List<ClassificationDto>> GetAllInactiveClassificationsAsync();
        #endregion

        #region Type
        public Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync();
        public Task<TypeModelDto> GetTypeModelByIdAsync(int id);
        public Task<List<TypeModelDto>> GetAllInactiveTypesAsync();
        #endregion

        #region Supplier
        public Task<IEnumerable<SupplierDTO>> GetAllSupplierAsync();
        public Task<SupplierDTO> GetSupplierByIdAsync(int id);
        #endregion

        #region Inventory
        public Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
        public Task<InventoryDto> GetInventoryByIdAsync(int id);
        public Task<List<InventoryDto>> GetAllAvailableInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<InventoryDto>> GetExpiredInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetCriticalStockInventoryAsync();
        public Task<List<InventoryDto>> GetInventoriesOutOfStockAsync();
        #endregion

        #region VAT
        public Task<IEnumerable<VatDto>> GetAllVATAsync();
        public Task<VatDto> GetVATByIdAsync(int id);
        #endregion

        #region PurchaseOrder
        public Task<IEnumerable<PurchaseOrderDTO>> GetAllPurchaseOrderAsync();
        public Task<PurchaseOrderDTO> GetPurchaseOrderByIdAsync(int id);
        public Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderBySupplierIdAsync(int supplierId);
        public Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderByStatusAsync(string status);
        public Task<List<PurchaseOrderDTO>> GetPendingPurchaseOrdersAsync();
        #endregion

        #region StockMovement
        public Task<IEnumerable<StockMovementDto>> GetAllStockMovementAsync();
        public Task<StockMovementDto> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByProductIdAsync(int productId);
        #endregion

        #region Reports
        // Sales Reports
        Task<SalesReportDto> GetSalesReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? employeeId = null,
            string paymentMethod = null);

        Task<byte[]> ExportSalesReportToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? employeeId = null,
            string paymentMethod = null,
            int? currentLoggedInUserId = null);

        // Inventory Reports
        Task<InventoryReportDto> GetInventoryReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string filterType = "all");

        Task<byte[]> ExportInventoryReportToExcelAsync(
            DateTime? startDate =null,
            DateTime? endDate = null,
            string filterType = "all",
            int? currentLoggedInUserId = null);

        // Fast-Moving Products
        Task<FastMovingProductReportDto> GetFastMovingProductsReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int topN = 20);

        Task<byte[]> ExportFastMovingProductsToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int topN = 20,
            int? currentLoggedInUserId = null);
        #endregion

        #region AdminDashboard
        public Task<decimal> GetTodaySalesAmountAsync();
        public Task<int> GetInventoryLowStockCountAsync();
        public Task<int> GetInventoryOutOfStockCountAsync();
        public Task<List<ProductSalesSummaryDto>> GetTopSellingProductsMonthlyAsync();
        #endregion
        
        #region BackUp Database
        Task<BackupListResponse> GetAllBackupsAsync();
        Task<BackupSettingsDto> GetBackupSettingsAsync();
        Task<byte[]?> DownloadBackupAsync(string fileName);
        #endregion

        #region StockHistory
        Task<StockHistoryResponse> GetStockHistoryAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? searchTerm);
        #endregion
    }
}
