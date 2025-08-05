using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
        //employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);

        //sales
        public Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id);

        //DISCOUNT
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);
    }
}
