using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
    }
}
