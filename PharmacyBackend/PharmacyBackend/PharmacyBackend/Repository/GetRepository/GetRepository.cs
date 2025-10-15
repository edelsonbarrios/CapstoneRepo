using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
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
            return await _context.Accounts
                .OrderByDescending(a => a.Id)
                .ToListAsync();
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

        #region Daily Sales
        public async Task<DailySalesSummaryDto> GetSummarySalesForToday()
        {
            var transactions = await _context.SalesTransaction
                .Include(st => st.Items)
                .Where(st => st.TransactionDate.Date >= DateTime.UtcNow.Date
                && st.TransactionDate.Date <= DateTime.UtcNow.Date)
                .ToListAsync();
            return new DailySalesSummaryDto
            {
                TotalSales = transactions.Sum(t => t.NetAmount),
                TotalTransactions = transactions.Count(),
                TotalItemsSold = transactions.SelectMany(t => t.Items).Sum(i => i.Quantity),
                UniqueProductsCount = transactions
                    .SelectMany(t => t.Items)
                    .Select(i => i.ProductId)
                    .Distinct()
                    .Count()
            };
        }

        public async Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId = null,
            string paymentMethod = null)
        {
            // Ensure we're comparing dates only (ignore time)
            var start = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
            // End of the day

            // Base query
            var query = _context.SalesTransaction
                .Include(st => st.Items)
                .Where(st => st.TransactionDate >= start && st.TransactionDate <= end);

            // Apply filters
            if (employeeId.HasValue)
            {
                query = query.Where(st => st.EmployeeID == employeeId.Value);
            }

            if(startDate == null || endDate == null)
            {
                query = _context.SalesTransaction
                    .Where(st => st.TransactionDate >= DateTime.UtcNow.Date
                    && st.TransactionDate < DateTime.UtcNow.Date.AddDays(1));
            }

            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query = query.Where(st => st.PaymentMethod == paymentMethod);
            }

            var transactions = await query.ToListAsync();

            // Calculate summary
            var summary = new DailySalesSummaryDto
            {
                TotalSales = transactions.Sum(t => t.NetAmount),
                TotalTransactions = transactions.Count,
                TotalItemsSold = transactions.SelectMany(t => t.Items).Sum(i => i.Quantity),
                UniqueProductsCount = transactions
                    .SelectMany(t => t.Items)
                    .Select(i => i.ProductId)
                    .Distinct()
                    .Count()
            };

            return summary;
        }

        public async Task<IEnumerable<ProductSalesSummaryDto>> GetProductSalesSummaryAsync(
            DateTime startDate,
            DateTime endDate)
        {
            // Ensure we're comparing dates only (ignore time)
            var start = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
            // End of the day

            // Get sales items within date range
            var salesData = await _context.SalesItem
                .Include(si => si.SalesTransaction)
                .Include(si => si.Product)
                    .ThenInclude(p => p.Inventory)
                .Where(si => si.SalesTransaction.TransactionDate >= start
                          && si.SalesTransaction.TransactionDate <= end)
                .GroupBy(si => new
                {
                    si.ProductId,
                    si.Product.ProductName
                })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(si => si.Quantity),
                    TotalSales = g.Sum(si => si.Subtotal),
                    Product = g.FirstOrDefault().Product
                })
                .ToListAsync();

            // Map to DTO and calculate available stock from current inventory
            var result = salesData.Select(sd => new ProductSalesSummaryDto
            {
                ProductId = sd.ProductId,
                ProductName = sd.ProductName,
                QuantitySold = sd.QuantitySold,
                AvailableStock = sd.Product?.Inventory?
                    .Where(i => i.IsActive && !i.IsExpired)
                    .Sum(i => i.TotalPiecesAvailable) ?? 0,
                TotalSales = sd.TotalSales
            }).ToList();

            return result;
        }

        public async Task<IEnumerable<SalesTransaction>> GetFilteredSalesTransactionsAsync(
            DateTime startDate,
            DateTime endDate,
            int? employeeId = null,
            string paymentMethod = null)
        {
            // Ensure we're comparing dates only (ignore time)
            var start = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc); // End of the day

            // Base query with includes
            var query = _context.SalesTransaction
                .Include(st => st.Items)
                    .ThenInclude(si => si.Product)
                .Include(st => st.Employee)
                .Include(st => st.Discount)
                .Include(st => st.VAT)
                .Where(st => st.TransactionDate >= start && st.TransactionDate <= end);

            // Apply filters
            if (employeeId.HasValue)
            {
                query = query.Where(st => st.EmployeeID == employeeId.Value);
            }

            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query = query.Where(st => st.PaymentMethod == paymentMethod);
            }

            // Order by date descending (newest first)
            query = query.OrderByDescending(st => st.TransactionDate);

            return await query.ToListAsync();
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
                    .Take(5)
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
                .OrderByDescending(d => d.Id)
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
                .OrderByDescending(p => p.DateCreated) 
                .ThenByDescending(p => p.ProductID)   
                .Where(p => p.IsActive == true)
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
            return await _context.Category
                .OrderByDescending(c => c.CategoryID)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }
        #endregion

        #region Brand
        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brand
                .OrderByDescending(b => b.BrandID)
                .ToListAsync();
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brand.FindAsync(id);
        }
        #endregion

        #region Generic
        public async Task<IEnumerable<Generic>> GetAllGenericAsync()
        {
            return await _context.Generic
                .OrderByDescending(g =>  g.GenericID)
                .ToListAsync();
        }
        public async Task<Generic> GetGenericByIdAsync(int id)
        {
            return await _context.Generic.FindAsync(id);
        }
        #endregion

        #region Formulation
        public async Task<IEnumerable<Formulation>> GetAllFormulationAsync()
        {
            return await _context.Formulation
                .OrderByDescending(f => f.FormulationID)
                .ToListAsync();
        }
        public async Task<Formulation> GetFormulationByIdAsync(int id)
        {
            return await _context.Formulation.FindAsync(id);
        }
        #endregion

        #region Classification
        public async Task<IEnumerable<Classification>> GetAllClassificationAsync()
        {
            return await _context.Classification
                .OrderByDescending(c => c.ClassificationID)
                .ToListAsync();
        }
        public async Task<Classification> GetClassificationByIdAsync(int id)
        {
            return await _context.Classification.FindAsync(id);
        }
        #endregion

        #region Type
        public async Task<IEnumerable<TypeModel>> GetAllTypeModelAsync()
        {
            return await _context.TypeModel
                .OrderByDescending(T => T.TypeID)
                .ToListAsync();
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
                .ThenInclude(p => p.Category)
                .Include(s => s.Products)
                .ThenInclude(p => p.Brand)
                .Include(s => s.Products)
                .ThenInclude(p => p.Generic)
                .Include(s => s.Products)
                .ThenInclude(p => p.Formulation)
                .Include(s => s.Products)
                .ThenInclude(p => p.Classification)
                .Include(s => s.Products)
                .ThenInclude(p => p.TypeModel)
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
                .ThenInclude(p => p.Category)
                .Include(i => i.Product.Brand)
                .Include(i => i.Product.Generic)
                .Include(i => i.Product.Formulation)
                .Include(i => i.Product.Classification)
                .Include(i => i.Product.TypeModel)
                .OrderByDescending(i => i.DateAdded)
                .ThenByDescending(i => i.InventoryID)
                .ToListAsync();
        }
        public async Task<List<Inventory>> GetAllAvailableInventoryAsync()
        {
            var inventories = await _context.Inventory
                .Include(i => i.Product)
                .ThenInclude(p => p.Category)
                .Include(i => i.Product.Brand)
                .Include(i => i.Product.Generic)
                .Include(i => i.Product.Formulation)
                .Include(i => i.Product.Classification)
                .Include(i => i.Product.TypeModel)
                .OrderByDescending(i => i.DateAdded)
                .ThenByDescending(i => i.InventoryID)
                .ToListAsync();

            return inventories
                .GroupBy(i => new { i.ProductID })
                .Select(g =>
                    g.Where(i => i.TotalPiecesAvailable > 0 && i.IsActive)
                    .OrderBy(i => i.ExpirationDate)
                    .FirstOrDefault()
                )
                .Where(i => i != null)
                .ToList();
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
                .Where(i => i.ExpirationDate < DateTime.UtcNow)
                .Take(4)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetNearExpiryInventoryAsync()
        {
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var nearExpiryDate = DateTime.SpecifyKind(DateTime.Now.AddDays(90), DateTimeKind.Utc);

            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.ExpirationDate <= nearExpiryDate && i.ExpirationDate > now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .Where(i => i.BoxQuantity <= i.ReorderLevel && i.IsActive == true)
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
                .Where(po => po.Status.Equals("Pending") && po.IsActive == true)
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
