using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Repository.UpdateRepository
{
    public class UpdateRepository : IUpdateRepositoryContracts
    {
        private readonly PharmacyContext _context;
        public UpdateRepository(PharmacyContext context)
        {
            _context = context;
        }
        //employee
        public async Task<bool> UpdateEmployeeAsync(Employee request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }

        //sales
        public async Task<bool> UpdateSalesTransactionAsync(SalesTransaction sale)
        {
            _context.SalesTransaction.Update(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        //Discount
        public async Task<bool> UpdateDiscountAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
