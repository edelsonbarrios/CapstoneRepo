using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        //employee
        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<bool> AuthenticationAsync(LogInRequest request);
        public string TokenGeneration(Account request);

        //sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);
    }
}
