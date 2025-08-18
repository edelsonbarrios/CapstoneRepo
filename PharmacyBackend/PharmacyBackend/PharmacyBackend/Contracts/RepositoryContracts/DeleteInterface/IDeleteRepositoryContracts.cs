namespace PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface
{
    public interface IDeleteRepositoryContracts
    {
        //employee
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);

        //sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);

        //Discount
        public Task<bool> DeleteDiscountByIdAsync(int id);

        //Product
        public Task<bool> DeleteProductByIdAsync(int id);

        //Category
        public Task<bool> DeleteCategoryByIdAsync(int id);

        //ItemDataControl

        //Brand
        public Task<bool> DeleteBrandByIdAsync(int id);

        //Generic
        public Task<bool> DeleteGenericByIdAsync(int id);

        //Formulation
        public Task<bool> DeleteFormulationByIdAsync(int id);

        //Classification
        public Task<bool> DeleteClassificationByIdAsync(int id);

        //Types
        public Task<bool> DeleteTypeModelByIdAsync(int id);


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
