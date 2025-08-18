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

        //employee
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
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
            return await _context.Product
               .Include(p => p.Category)
               .Include(p => p.Brand)
               .Include(p => p.Generic)
               .Include(p => p.Formulation)
               .Include(p => p.Classification)
               .Include(p => p.TypeModel)
               .Include(p => p.Supplier)
               .Include(p => p.Inventory.Where(i => i.IsActive))
               .ToListAsync();
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

        //Supplier
        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            return await _context.Supplier.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Supplier.FindAsync(id);
        }

        //Inventory

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.InventoryID == id);
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductIdAsync(int productId)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.ProductID == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetExpiredInventoryAsync()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.ExpirationDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetNearExpiryInventoryAsync()
        {
            var nearExpiryDate = DateTime.Now.AddDays(30);
            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.ExpirationDate <= nearExpiryDate && i.ExpirationDate > DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.QuantityOnHand <= i.ReorderLevel)
                .ToListAsync();
        }

        //VAT
        public async Task<IEnumerable<VAT>> GetAllVATAsync()
        {
            return await _context.VAT.ToListAsync();
        }

        public async Task<VAT> GetVATByIdAsync(int id)
        {
            return await _context.VAT.FindAsync(id);
        }

        //PurchaseOrder
        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync()
        {
            return await _context.PurchaseOrder
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Product)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id)
        {
            return await _context.PurchaseOrder
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Product)
                .FirstOrDefaultAsync(po => po.PurchaseOrderID == id);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderBySupplierIdAsync(int supplierId)
        {
            return await _context.PurchaseOrder
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Product)
                .Where(po => po.SupplierID == supplierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrderByStatusAsync(string status)
        {
            return await _context.PurchaseOrder
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Product)
                .Where(po => po.Status == status)
                .ToListAsync();
        }

        //StockMovement
        public async Task<IEnumerable<StockMovement>> GetAllStockMovementAsync()
        {
            return await _context.StockMovement
                .Include(sm => sm.Inventory)
                    .ThenInclude(i => i.Product)
                .Include(sm => sm.Employee)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync();
        }

        public async Task<StockMovement> GetStockMovementByIdAsync(int id)
        {
            return await _context.StockMovement
                .Include(sm => sm.Inventory)
                    .ThenInclude(i => i.Product)
                .Include(sm => sm.Employee)
                .FirstOrDefaultAsync(sm => sm.StockMovementID == id);
        }

        public async Task<IEnumerable<StockMovement>> GetStockMovementByInventoryIdAsync(int inventoryId)
        {
            return await _context.StockMovement
                .Include(sm => sm.Inventory)
                    .ThenInclude(i => i.Product)
                .Include(sm => sm.Employee)
                .Where(sm => sm.InventoryID == inventoryId)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetStockMovementByProductIdAsync(int productId)
        {
            return await _context.StockMovement
                .Include(sm => sm.Inventory)
                    .ThenInclude(i => i.Product)
                .Include(sm => sm.Employee)
                .Where(sm => sm.Inventory.ProductID == productId)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync();
        }
    }
}
