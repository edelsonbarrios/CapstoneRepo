using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.GetInterface
{
    public interface IGetRepositoryContracts
    {
        //employee
        public Task<List<Employee>> GetAllEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);

        //sales
        Task<List<SalesTransaction>> GetAllSalesTransactionsAsync();
        Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id);
    }
}
