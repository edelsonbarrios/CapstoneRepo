namespace PharmacyBackend.Contracts.ServiceContracts.DeleteInterface
{
    public interface IDeleteServiceContracts
    {
        //EMPLOYEE
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);

        //sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);

        //DISCOUNT
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

        //Type
        public Task<bool> DeleteTypeModelByIdAsync(int id);

    }
}
