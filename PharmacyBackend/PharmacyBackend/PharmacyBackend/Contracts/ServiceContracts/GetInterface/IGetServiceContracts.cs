using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
        //employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);

        //sales
        Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        Task<SalesTransactionDto?> GetSalesTransactionByIdAsync(int id);
    }
}
