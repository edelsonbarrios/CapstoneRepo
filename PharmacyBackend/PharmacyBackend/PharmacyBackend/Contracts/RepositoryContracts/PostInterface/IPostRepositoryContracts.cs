using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Contracts.RepositoryContracts.PostInterface
{
    public interface IPostRepositoryContracts
    {
        public Task<int> CreateEmployeeAsync(Employee request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(Account request);
    }
}
