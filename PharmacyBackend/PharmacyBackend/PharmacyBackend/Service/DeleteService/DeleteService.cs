using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.Repository.DELETE;

namespace PharmacyBackend.Service.DeleteService
{
    public class DeleteService : IDeleteServiceContracts
    {
        private readonly IDeleteRepositoryContracts _deleteRepo;

        public DeleteService(IDeleteRepositoryContracts deleteRepo)
        {
            _deleteRepo = deleteRepo;
        }
        //employee
        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            var deleteEmployee = await _deleteRepo.DeleteEmployeeByIdAsync(id);
            if (!deleteEmployee)
                return false;
            return true;
        }
        //sales
        public async Task<bool> DeleteSalesTransactionByIdAsync(int id)
        {
            var deleted = await _deleteRepo.DeleteSalesTransactionByIdAsync(id);
            return deleted;
        }
    }
}
