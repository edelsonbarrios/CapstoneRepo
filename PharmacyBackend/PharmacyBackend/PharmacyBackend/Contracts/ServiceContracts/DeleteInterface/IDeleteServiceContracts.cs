namespace PharmacyBackend.Contracts.ServiceContracts.DeleteInterface
{
    public interface IDeleteServiceContracts
    {
        //EMPLOYEE
        public Task<bool> DeleteEmployeeByIdAsync(int id);

        //sales
        public Task<bool> DeleteSalesTransactionByIdAsync(int id);

        //DISCOUNT
        public Task<bool> DeleteDiscountByIdAsync(int id);

    }
}
