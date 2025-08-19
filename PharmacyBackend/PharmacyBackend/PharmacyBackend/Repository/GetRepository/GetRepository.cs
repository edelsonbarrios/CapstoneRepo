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

        #region Account
        public async Task<bool> AccountUsernameValidation(string userName)
        {
            return await _context.Accounts.AnyAsync(a => a.Username.Equals(userName));
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
        #endregion

        #region Employee
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<List<Employee>> GetEmployeeByLastName(string lastName)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .AsNoTracking()
                .Where(e => e.LastName.Contains(lastName)).ToListAsync();
        }
        #endregion

        #region Sales
        public async Task<List<SalesTransaction>> GetAllSalesTransactionsAsync()
        {
            return await _context.SalesTransaction
                .Include(t => t.Discount)
                .Include(t => t.Items)
                  .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id)
        {
            return await _context.SalesTransaction
                .Include(t => t.Discount)
                .Include(t => t.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        #endregion

        #region Discount
        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }
        #endregion

        #region Product
        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }
        #endregion

        #region Category
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }
        #endregion

        #region Brand
        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brand.ToListAsync();
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brand.FindAsync(id);
        }
        #endregion

        #region Generic
        public async Task<IEnumerable<Generic>> GetAllGenericAsync()
        {
            return await _context.Generic.ToListAsync();
        }
        public async Task<Generic> GetGenericByIdAsync(int id)
        {
            return await _context.Generic.FindAsync(id);
        }
        #endregion

        #region Formulation
        public async Task<IEnumerable<Formulation>> GetAllFormulationAsync()
        {
            return await _context.Formulation.ToListAsync();
        }
        public async Task<Formulation> GetFormulationByIdAsync(int id)
        {
            return await _context.Formulation.FindAsync(id);
        }
        #endregion

        #region Classification
        public async Task<IEnumerable<Classification>> GetAllClassificationAsync()
        {
            return await _context.Classification.ToListAsync();
        }
        public async Task<Classification> GetClassificationByIdAsync(int id)
        {
            return await _context.Classification.FindAsync(id);
        }
        #endregion

        #region Type
        public async Task<IEnumerable<TypeModel>> GetAllTypeModelAsync()
        {
            return await _context.TypeModel.ToListAsync();
        }
        public async Task<TypeModel> GetTypeModelByIdAsync(int id)
        {
            return await _context.TypeModel.FindAsync(id);
        }
        #endregion

        #region Role
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        #endregion

    }
}
