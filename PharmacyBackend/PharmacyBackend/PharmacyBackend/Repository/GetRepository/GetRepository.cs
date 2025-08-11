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

        //Discount
        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        //Product
        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _context.Product.ToListAsync();
        }
        
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        //Category
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }

        //ItemDataControl

        //Brand
        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brand.ToListAsync();
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brand.FindAsync(id);
        }

        //Generic
        public async Task<IEnumerable<Generic>> GetAllGenericAsync()
        {
            return await _context.Generic.ToListAsync();
        }
        public async Task<Generic> GetGenericByIdAsync(int id)
        {
            return await _context.Generic.FindAsync(id);
        }

        //Formulation
        public async Task<IEnumerable<Formulation>> GetAllFormulationAsync()
        {
            return await _context.Formulation.ToListAsync();
        }
        public async Task<Formulation> GetFormulationByIdAsync(int id)
        {
            return await _context.Formulation.FindAsync(id);
        }

        //Classification
        public async Task<IEnumerable<Classification>> GetAllClassificationAsync()
        {
            return await _context.Classification.ToListAsync();
        }
        public async Task<Classification> GetClassificationByIdAsync(int id)
        {
            return await _context.Classification.FindAsync(id);
        }

        //Type
        public async Task<IEnumerable<TypeModel>> GetAllTypeModelAsync()
        {
            return await _context.TypeModel.ToListAsync();
        }
        public async Task<TypeModel> GetTypeModelByIdAsync(int id)
        {
            return await _context.TypeModel.FindAsync(id);
        }
    }
}
