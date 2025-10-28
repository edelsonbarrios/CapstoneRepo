using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;
using System.Runtime.CompilerServices;

namespace PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface
{
    public interface IUpdateRepositoryContracts
    {

        #region Employee&Account
        public Task<bool> UpdateEmployeeAsync(Employee request);
        public Task<bool> UpdateAccountAsync(Account request);
        #endregion

        #region Sales
        public Task<SalesTransaction?> UpdateSalesTransactionAsync(SalesTransaction salesTransaction);
        public Task<SalesItem?> UpdateSalesItemAsync(SalesItem salesItem);

        #endregion

        #region Discount
        public Task<bool> UpdateDiscountAsync(Discount discount);
        #endregion

        #region Product
        public Task<bool> UpdateProductAsync(Product product);
        #endregion

        #region Category
        public Task<bool> UpdateCategoryAsync(Category category);
        #endregion

        #region Brand
        public Task<bool> UpdateBrandAsync(Brand brand);
        #endregion

        #region Generic
        public Task<bool> UpdateGenericAsync(Generic generic);
        #endregion

        #region Formulation
        public Task<bool> UpdateFormulationAsync(Formulation formulation);
        #endregion

        #region Classification
        public Task<bool> UpdateClassificationAsync(Classification classification);
        #endregion

        #region TypeModel
        public Task<bool> UpdateTypeModelAsync(TypeModel typeModel);
        #endregion

        #region Supplier
        public Task<bool> UpdateSupplierAsync(Supplier supplier);
        #endregion

        #region Inventory
        public Task<bool> UpdateInventoryAsync(Inventory inventory);
        public Task<bool> ValidateInventoryStocksAsync();
        #endregion

        #region VAT
        public Task<bool> UpdateVATAsync(VAT vat);
        #endregion

        #region PurchaseOrder
        public Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        #endregion

        #region BackUp Database
        Task<bool> UpdateBackupSettingsAsync(BackupSettingsDto settings);
        #endregion
    }
}
