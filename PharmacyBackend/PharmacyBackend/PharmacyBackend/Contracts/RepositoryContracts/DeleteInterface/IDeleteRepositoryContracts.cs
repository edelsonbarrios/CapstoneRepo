    namespace PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface
    {
        public interface IDeleteRepositoryContracts
        {
            //employee
            public Task<bool> DeleteEmployeeByIdAsync(int id);

            //sales
            public Task<bool> DeleteSalesTransactionByIdAsync(int id);
        }
    }
