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
        public async Task<Account> SearchAccountByUsernameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username.StartsWith(userName));
        }
        #endregion

        #region Employee
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Where(e => e.IsActive == true)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<List<Employee>> GetEmployeeByLastName(string lastName)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .AsNoTracking()
                .Where(e => e.LastName.StartsWith(lastName) && e.IsActive == true)
                .ToListAsync();
        }
        public async Task<List<Employee>> GetFirstDisplayEmployees()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .OrderBy(e => e.LastName)
                .Take(7)
                .Where(e => e.IsActive == true)
                .ToListAsync();
        }
        #endregion

        #region EmployeeHistory
        public async Task<List<EmployeeHistory>> GetAllEmployeeHistoryAsync()
        {
            return await _context.EmployeeHistory
                .AsNoTracking()
                .OrderByDescending(eh => eh.PerformedAt)
                .Include(eh => eh.Employee)
                .Include(eh => eh.PerformedByEmployee)
                .ToListAsync();
        }
        public async Task<EmployeeHistory> GetEmployeeHistoryByIdAsync(int employeeHistoryId)
        {
            return await _context.EmployeeHistory
                .AsNoTracking()
                .Include(eh => eh.Employee)
                .Include(eh => eh.PerformedByEmployee)
                .OrderByDescending(eh => eh.PerformedAt)
                .FirstOrDefaultAsync(eh => eh.Id == employeeHistoryId);
        }
        public async Task<List<EmployeeHistory>> GetDisplayEmployeeHistoryAsync()
        {
            return await _context.EmployeeHistory
                .AsNoTracking()
                .Include(eh => eh.Employee)
                .Include(eh => eh.PerformedByEmployee)
                .OrderByDescending(eh => eh.PerformedAt)
                .Take(8)
                .ToListAsync();
        }
        #endregion

        #region Sales
        // Sales Transaction methods
        public async Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id)
        {
            try
            {
                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .FirstOrDefaultAsync(st => st.Id == id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales transaction with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SalesTransaction>> GetAllSalesTransactionsAsync()
        {
            try
            {
                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving all sales transactions: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByEmployeeIdAsync(int employeeId)
        {
            try
            {
                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .Where(st => st.EmployeeID == employeeId)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales transactions for employee ID {employeeId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .Where(st => st.TransactionDate.Date >= startDate.Date && st.TransactionDate.Date <= endDate.Date)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales transactions for date range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SalesTransaction>> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber)
        {
            try
            {
                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .Where(st => st.InvoiceNumber.Contains(invoiceNumber))
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales transactions for invoice number {invoiceNumber}: {ex.Message}", ex);
            }
        }

        // Sales Item methods
        public async Task<SalesItem?> GetSalesItemByIdAsync(int id)
        {
            try
            {
                return await _context.SalesItem
                    .Include(si => si.Product)
                    .Include(si => si.SalesTransaction)
                    .FirstOrDefaultAsync(si => si.Id == id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales item with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SalesItem>> GetSalesItemsByTransactionIdAsync(int transactionId)
        {
            try
            {
                return await _context.SalesItem
                    .Include(si => si.Product)
                    .Where(si => si.SalesTransactionId == transactionId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving sales items for transaction ID {transactionId}: {ex.Message}", ex);
            }
        }
        #endregion

        #region Discount
        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            return await _context.Discounts
                .Where(d => d.IsActive == true)
                .ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive == true);
        }
        #endregion

        #region Product
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

        #region Supplier
        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            return await _context.Supplier
                .Include(s => s.Products)
                .Where(s => s.IsActive == true)
                .ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Supplier
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        #endregion

        #region Inventory
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
        #endregion

        #region VAT
        public async Task<IEnumerable<VAT>> GetAllVATAsync()
        {
            return await _context.VAT
                .Where(v => v.IsActive == true)
                .ToListAsync();
        }

        public async Task<VAT> GetVATByIdAsync(int id)
        {
            return await _context.VAT.FindAsync(id);
        }
        #endregion

        #region PurchaseOrder
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
                .FirstOrDefaultAsync(po => po.Id == id);
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
        public async Task<List<PurchaseOrder>> GetPendingPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrder
                .Include(po => po.Supplier)
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Product)
                .Where(po => po.Status.Equals("Pending"))
                .ToListAsync();
        }
        #endregion

        #region StockMovement
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
        #endregion

    }
}
