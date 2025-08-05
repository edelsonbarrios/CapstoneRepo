using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Repository.GET
{
    public class GetRepository : IGetRepositoryContracts
    {
        private readonly PharmacyContext _context;
        public GetRepository(PharmacyContext context)
        {
            _context = context;
        }
        //employee
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        //sales
        public async Task<List<SalesTransaction>> GetAllSalesTransactionsAsync()
        {
            return await _context.SalesTransaction.ToListAsync();
        }

        public async Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id)
        {
            return await _context.SalesTransaction.FindAsync(id);
        }

        //Discount
        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }
    }
}
