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
        public Task<bool> ValidateDiscountNameAsync(string name, int discountId);
        #endregion

        #region Product
        public Task<bool> UpdateProductAsync(Product product);
        #endregion

        #region Category
        public Task<bool> UpdateCategoryAsync(Category category);
        public Task<bool> ValidateCategoryNameAsync(string name, int categoryId);
        #endregion

        #region Brand
        public Task<bool> UpdateBrandAsync(Brand brand);
        public Task<bool> ValidateBrandNameAsync(string name, int brandId);
        #endregion

        #region Generic
        public Task<bool> UpdateGenericAsync(Generic generic);
        public Task<bool> ValidateGenericNameAsync(string name, int genericId);
        #endregion

        #region Formulation
        public Task<bool> UpdateFormulationAsync(Formulation formulation);
        public Task<bool> ValidateFormulationNameAsync(string name, int formulationId);
        #endregion

        #region Classification
        public Task<bool> UpdateClassificationAsync(Classification classification);
        public Task<bool> ValidateClassificationNameAsync(string name, int classificationId);
        #endregion

        #region TypeModel
        public Task<bool> UpdateTypeModelAsync(TypeModel typeModel);
        public Task<bool> ValidateTypeNameAsync(string name, int typeId);
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
        public Task<bool> ValidateVATNameAsync(string name, int vatId);
        #endregion

        #region PurchaseOrder
        public Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        #endregion

        #region BackUp Database
        Task<bool> UpdateBackupSettingsAsync(BackupSettingsDto settings);
        #endregion
    }
}
