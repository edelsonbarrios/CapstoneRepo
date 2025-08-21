using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface
{
    public interface IUpdateRepositoryContracts
    {

        #region Employee
        public Task<bool> UpdateEmployeeAsync(Employee request);
        public Task<bool> UpdateAccountAsync(Account request);
        #endregion

        #region Sales
        public Task<bool> UpdateSalesTransactionAsync(SalesTransaction transaction);
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
        #endregion

        #region VAT
        public Task<bool> UpdateVATAsync(VAT vat);
        #endregion

        #region PurchaseOrder
        public Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        #endregion

    }
}
