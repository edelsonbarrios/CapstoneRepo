using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.PostInterface
{
    public interface IPostRepositoryContracts
    {
        //employee
        public Task<bool> CreateEmployeeAsync(Employee request);
        public Task<bool> AuthenticationAsync(LogInRequest request);
        public string TokenGeneration(Account request);

        //sales
        public Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction transaction);
    }
}
