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
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
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
        #endregion

        #region Sales
        public Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id);
        #endregion

        #region Discount
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);
        #endregion

        #region Product
        public Task<IEnumerable<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        #endregion

        #region Category
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);
        #endregion

        #region Brand
        public Task<IEnumerable<BrandDto>> GetAllBrandAsync();
        public Task<BrandDto> GetBrandByIdAsync(int id);
        #endregion

        #region Generic
        public Task<IEnumerable<GenericDto>> GetAllGenericAsync();
        public Task<GenericDto> GetGenericByIdAsync(int id);
        #endregion

        #region Formulation
        public Task<IEnumerable<FormulationDto>> GetAllFormulationAsync();
        public Task<FormulationDto> GetFormulationByIdAsync(int id);
        #endregion

        #region Classification
        public Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync();
        public Task<ClassificationDto> GetClassificationByIdAsync(int id);
        #endregion

        #region Type
        public Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync();
        public Task<TypeModelDto> GetTypeModelByIdAsync(int id);
        #endregion

        #region Supplier
        public Task<IEnumerable<SupplierDto>> GetAllSupplierAsync();
        public Task<SupplierDto> GetSupplierByIdAsync(int id);
        #endregion

        #region Inventory
        public Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
        public Task<InventoryDto> GetInventoryByIdAsync(int id);
        public Task<IEnumerable<InventoryDto>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<InventoryDto>> GetExpiredInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetCriticalStockInventoryAsync();
        #endregion

        #region VAT
        public Task<IEnumerable<VatDto>> GetAllVATAsync();
        public Task<VatDto> GetVATByIdAsync(int id);
        #endregion

        #region PurchaseOrder
        public Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrderAsync();
        public Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id);
        public Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderBySupplierIdAsync(int supplierId);
        public Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderByStatusAsync(string status);
        #endregion

        #region StockMovement
        public Task<IEnumerable<StockMovementDto>> GetAllStockMovementAsync();
        public Task<StockMovementDto> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByProductIdAsync(int productId);
        #endregion
    }
}
