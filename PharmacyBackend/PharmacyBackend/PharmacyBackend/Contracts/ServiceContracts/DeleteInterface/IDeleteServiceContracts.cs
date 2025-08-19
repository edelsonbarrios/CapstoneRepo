namespace PharmacyBackend.Contracts.ServiceContracts.DeleteInterface
{
    public interface IDeleteServiceContracts
    {
        //EMPLOYEE
        #region Employee
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);
        #endregion

        //sales
        #region Sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);
        #endregion

        //DISCOUNT
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

        //Type
        #region Type
        public Task<bool> DeleteTypeModelByIdAsync(int id);
        #endregion

    }
}
