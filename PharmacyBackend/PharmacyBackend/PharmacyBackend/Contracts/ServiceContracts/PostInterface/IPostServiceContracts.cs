using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<bool> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
    }
}
