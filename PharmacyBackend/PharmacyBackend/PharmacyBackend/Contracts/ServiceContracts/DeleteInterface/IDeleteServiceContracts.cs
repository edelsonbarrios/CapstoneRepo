namespace PharmacyBackend.Contracts.ServiceContracts.DeleteInterface
{
    public interface IDeleteServiceContracts
    {
        public Task<bool> DeleteEmployeeByIdAsync(int id);

        //sales
        Task<bool> DeleteSalesTransactionByIdAsync(int id);
    }
}
