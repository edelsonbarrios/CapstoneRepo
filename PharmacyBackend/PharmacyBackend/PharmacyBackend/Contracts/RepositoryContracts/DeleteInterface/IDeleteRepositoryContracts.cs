namespace PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface
{
    public interface IDeleteRepositoryContracts
    {
        //employee
        #region Employee
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);
        #endregion

        //sales
        #region Sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);
        #endregion

        //Discount
        #region Discount
        public Task<bool> DeleteDiscountByIdAsync(int id);
        #endregion

        //Product
        #region Product
        public Task<bool> DeleteProductByIdAsync(int id);
        #endregion

        //Category
        #region Category
        public Task<bool> DeleteCategoryByIdAsync(int id);
        #endregion

        //ItemDataControl

        //Brand
        #region Brand
        public Task<bool> DeleteBrandByIdAsync(int id);
        #endregion

        //Generic
        #region Generic
        public Task<bool> DeleteGenericByIdAsync(int id);
        #endregion

        //Formulation
        #region Formulation
        public Task<bool> DeleteFormulationByIdAsync(int id);
        #endregion

        //Classification
        #region Classification
        public Task<bool> DeleteClassificationByIdAsync(int id);
        #endregion

        //Types
        #region Types
        public Task<bool> DeleteTypeModelByIdAsync(int id);
        #endregion


        //Supplier
        public Task<bool> DeleteSupplierByIdAsync(int id);

        //Inventory
        public Task<bool> DeleteInventoryByIdAsync(int id);

        //VAT
        public Task<bool> DeleteVATByIdAsync(int id);

        //PurchaseOrder
        public Task<bool> DeletePurchaseOrderByIdAsync(int id);
    }
}
