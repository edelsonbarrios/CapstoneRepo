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
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
        //employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        public Task<List<AccountDTO>> GetAllAccountsAsync();
        public Task<AccountDTO> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);

        //sales
        public Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id);

        //DISCOUNT
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);

        //PRODUCT
        public Task<IEnumerable<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);

        //Category
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);

        //ItemDataControl
        //Brand
        public Task<IEnumerable<BrandDto>> GetAllBrandAsync();
        public Task<BrandDto> GetBrandByIdAsync(int id);

        //Generic
        public Task<IEnumerable<GenericDto>> GetAllGenericAsync();
        public Task<GenericDto> GetGenericByIdAsync(int id);

        //Formulation
        public Task<IEnumerable<FormulationDto>> GetAllFormulationAsync();
        public Task<FormulationDto> GetFormulationByIdAsync(int id);

        //Classificaton
        public Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync();
        public Task<ClassificationDto> GetClassificationByIdAsync(int id);

        //Type
        public Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync();
        public Task<TypeModelDto> GetTypeModelByIdAsync(int id);

        //Supplier
        public Task<IEnumerable<SupplierDto>> GetAllSupplierAsync();
        public Task<SupplierDto> GetSupplierByIdAsync(int id);

        //Inventory
        public Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
        public Task<InventoryDto> GetInventoryByIdAsync(int id);
        public Task<IEnumerable<InventoryDto>> GetInventoryByProductIdAsync(int productId);
        public Task<IEnumerable<InventoryDto>> GetExpiredInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetNearExpiryInventoryAsync();
        public Task<IEnumerable<InventoryDto>> GetCriticalStockInventoryAsync();

        //VAT
        public Task<IEnumerable<VatDto>> GetAllVATAsync();
        public Task<VatDto> GetVATByIdAsync(int id);

        //PurchaseOrder
        public Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrderAsync();
        public Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id);
        public Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderBySupplierIdAsync(int supplierId);
        public Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderByStatusAsync(string status);

        //StockMovement
        public Task<IEnumerable<StockMovementDto>> GetAllStockMovementAsync();
        public Task<StockMovementDto> GetStockMovementByIdAsync(int id);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByInventoryIdAsync(int inventoryId);
        public Task<IEnumerable<StockMovementDto>> GetStockMovementByProductIdAsync(int productId);

    }
}
