using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Contracts.ServiceContracts.UpdateInterface
{
    public interface IUpdateServiceContracts
    {
        public Task<bool> UpdateEmployeeAsync(UpdateEmployeeRequest request);
        public Task<bool> UpdateAccountAsync(UpdateAccountRequest request);
    }
}
