namespace PharmacyBackend.Contracts.ServiceContracts.DeleteInterface
{
    public interface IDeleteServiceContracts
    {
        #region Employee
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);
        #endregion

        #region Sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);
        #endregion

        #region Discount
        public Task<bool> DeleteDiscountByIdAsync(int id);
        #endregion

        #region Product
        public Task<bool> DeleteProductByIdAsync(int id);
        #endregion

        #region Category
        public Task<bool> DeleteCategoryByIdAsync(int id);
        #endregion

        #region Brand
        public Task<bool> DeleteBrandByIdAsync(int id);
        #endregion

        #region Generic
        public Task<bool> DeleteGenericByIdAsync(int id);
        #endregion

        #region Formulation
        public Task<bool> DeleteFormulationByIdAsync(int id);
        #endregion

        #region Classification
        public Task<bool> DeleteClassificationByIdAsync(int id);
        #endregion

        #region Type
        public Task<bool> DeleteTypeModelByIdAsync(int id);
        #endregion

        #region Supplier
        public Task<bool> DeleteSupplierByIdAsync(int id);
        #endregion

        #region Inventory
        public Task<bool> DeleteInventoryByIdAsync(int id);
        #endregion

        #region VAT
        public Task<bool> DeleteVATByIdAsync(int id);
        #endregion

        #region PurchaseOrder
        public Task<bool> DeletePurchaseOrderByIdAsync(int id);
        #endregion

    }
}
