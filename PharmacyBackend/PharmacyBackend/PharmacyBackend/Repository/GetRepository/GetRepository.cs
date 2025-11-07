using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ReportsDTOs;
using PharmacyBackend.DTOs.StockHistoryDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;
using System.Runtime.InteropServices;

namespace PharmacyBackend.Repository.GET
{
    public class GetRepository : IGetRepositoryContracts
    {
        private readonly PharmacyContext _context;
        private readonly IConfiguration _config;
        public GetRepository(PharmacyContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        #region Account
        public async Task<bool> AccountUsernameValidation(string userName)
        {
            return await _context.Accounts.AnyAsync(a => a.Username.Equals(userName));
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .OrderByDescending(a => a.Id)
                .Where(a => a.IsActive == true)
                .ToListAsync();
        }
        public async Task<Account> SearchAccountByUsernameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username.StartsWith(userName));
        }
        public async Task<Account> GetAccountByEmployeeIdAsync(int employeeId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId);
        }
        #endregion

        #region Employee
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Account)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Where(e => e.IsActive == true)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Account)
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
            var phNow = DateTime.UtcNow.AddHours(8);

            // Get start and end of today in PH time
            var todayStart = phNow.Date;
            var tomorrowStart = todayStart.AddDays(1);

            var transactions = await _context.SalesTransaction
                .Include(st => st.Items)
                .Where(st => st.TransactionDate >= todayStart && st.TransactionDate < tomorrowStart)
                .ToListAsync();

            return new DailySalesSummaryDto
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
        }

        public async Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId = null,
            string paymentMethod = null)
        {
            // Handle null dates first - default to today
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = DateTime.UtcNow.Date;
                endDate = DateTime.UtcNow.Date;
            }

            // Ensure we're comparing dates only (ignore time)
            var start = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            // Base query
            var query = _context.SalesTransaction
                .Include(st => st.Items)
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
            // ✅ If no dates provided, default to current week (Monday → Sunday)
            if (startDate == default || endDate == default)
            {
                var today = DateTime.UtcNow.Date;
                int daysSinceMonday = ((int)today.DayOfWeek + 6) % 7; // Convert Sunday=0 → 6
                startDate = today.AddDays(-daysSinceMonday); // Monday of current week
                endDate = startDate.AddDays(6); // Sunday of current week
            }

            // Ensure date comparisons ignore time
            var start = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc); // end of day

            // ✅ Fetch sales items within the range
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

            // ✅ Map to DTO
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

        public async Task<bool> ReferenceNumberValidationAsync(string refNum) 
        {
          return await _context.SalesTransaction
            .AnyAsync(s => s.ReferenceNumber.Equals(refNum));
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

        public async Task<List<Product>> GetAllInactiveProductsAsync() 
        {
          var inactiveProducts = await _context.Product
            .Where(p => p.IsActive == false)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Generic)
            .Include(p => p.Formulation) 
            .Include(p => p.Classification)
            .Include(p => p.TypeModel)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

          return inactiveProducts;
        }

        public async Task<Product> GetProductByNameAsync(string name) 
        {
          return await _context.Product
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductName.Equals(name));
        }
        #endregion

        #region Category

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
          return await _context.Category
            .OrderByDescending(c => c.CategoryID)
            .Where(c => c.IsActive == true)
            .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
          return await _context.Category.FindAsync(id);
        }

        public async Task<List<Category>> GetAllInactiveCategoriesAsync() 
        {
          return await _context.Category
            .Where(c => c.IsActive == false)
            .ToListAsync();
        }

        #endregion

        #region Brand
        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
          return await _context.Brand
            .OrderByDescending(b => b.BrandID)
            .Where(b => b.IsActive == true)
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
            .Where(g => g.IsActive == true)
            .ToListAsync();
        }
        public async Task<Generic> GetGenericByIdAsync(int id)
        {
          return await _context.Generic.AsNoTracking().FirstOrDefaultAsync(g => g.GenericID == id);
        }
        #endregion

        #region Formulation
        public async Task<IEnumerable<Formulation>> GetAllFormulationAsync()
        {
          return await _context.Formulation
            .OrderByDescending(f => f.FormulationID)
            .Where(f => f.IsActive == true)
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
            .Where(c => c.IsActive == true)
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
            .Where(t => t.IsActive == true)
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
            .OrderByDescending(s => s.Id)
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
            .Where(i => i.IsActive == true)
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
                g.Where(i => i.TotalPiecesAvailable > 0 && i.IsActive && i.ExpirationDate >= DateTime.Now)
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
            .ThenInclude(p => p.Category)
            .Include(i => i.Product.Brand)
            .Include(i => i.Product.Generic)
            .Include(i => i.Product.Formulation)
            .Include(i => i.Product.Classification)
            .Include(i => i.Product.TypeModel)
            .OrderByDescending(i => i.DateAdded)
            .ThenByDescending(i => i.InventoryID)
            .Where(i => i.ExpirationDate <= nearExpiryDate && i.ExpirationDate > now)
            .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetCriticalStockInventoryAsync()
        {
            var now = DateTime.UtcNow;
            // Get all active, non-expired inventory grouped by product
            var inventoryByProduct = await _context.Inventory
                .Include(i => i.Product)
                    .ThenInclude(p => p.Category)
                .Include(i => i.Product.Brand)
                .Include(i => i.Product.Generic)
                .Include(i => i.Product.Formulation)
                .Include(i => i.Product.Classification)
                .Include(i => i.Product.TypeModel)
                .Where(i => i.IsActive && i.ExpirationDate >= now)
                .ToListAsync();

            // Group by product and calculate total stock
            var criticalProducts = inventoryByProduct
                .GroupBy(i => i.ProductID)
                .Where(g =>
                {
                    var product = g.First().Product;
                    var reorderLevel = product.ReorderLevel;

                    // Calculate total stock based on unit measurement
                    int totalStock;
                    if (product.UnitMeasurement == "Multi-Level" || product.UnitMeasurement == "Dual-Level")
                    {
                        // Sum all box quantities for box-based products
                        totalStock = g.Sum(i => i.BoxQuantity);
                    }
                    else // Single-Level (pieces only)
                    {
                        // Sum total pieces available
                        totalStock = g.Sum(i => i.TotalPiecesAvailable);
                    }

                    return totalStock <= reorderLevel;
                })
                .SelectMany(g => g) // Flatten back to individual inventory items
                .ToList();

            return criticalProducts;
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
          var pendings = await _context.PurchaseOrder
            .Include(po => po.Supplier)
            .Include(po => po.PurchaseOrderItems)
            .ThenInclude(poi => poi.Product)
            .Where(po => po.Status.Equals("Pending") && po.IsActive == true)
            .ToListAsync();

          return pendings;
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

        #region Reports

        public async Task<IEnumerable<SalesTransaction>> GetSalesReportDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId,
            string paymentMethod)
        {
          var query = _context.SalesTransaction
            .Include(st => st.Employee)
            .Include(st => st.Discount)
            .Include(st => st.VAT)
            .Include(st => st.Items)
            .ThenInclude(si => si.Product)
            .AsQueryable();

          // Apply date filter only if BOTH dates are provided
          if(startDate.HasValue && !endDate.HasValue)
          {
            var startDateUtc = startDate.Value.Date;
            query = query.Where(st => st.TransactionDate.Date >= startDateUtc);
          }

          if (startDate.HasValue && endDate.HasValue)
          {
            var startDateUtc = startDate.Value.Date;
            var endDateUtc = endDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(st => st.TransactionDate >= startDateUtc && st.TransactionDate <= endDateUtc);
          }
          // If no dates provided, show ALL transactions (no date filter)

          if (employeeId.HasValue)
          {
            query = query.Where(st => st.EmployeeID == employeeId.Value);
          }

          if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod.ToLower() != "all methods")
          {
            query = query.Where(st => st.PaymentMethod.ToLower() == paymentMethod.ToLower());
          }

          return await query
            .OrderByDescending(st => st.TransactionDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoryReportDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            string filterType)
        {
          var now = DateTime.UtcNow.Date;

          var query = _context.Inventory
            .Include(i => i.Product)
            .ThenInclude(p => p.Category)
            .Include(i => i.Product)
            .ThenInclude(p => p.Brand)
            .Include(i => i.Product)
            .ThenInclude(p => p.Generic)
            .Include(i => i.Product)
            .ThenInclude(p => p.Formulation)
            .Include(i => i.Product)
            .ThenInclude(p => p.Classification)
            .Include(i => i.Product)
            .ThenInclude(p => p.TypeModel)
            .AsQueryable();

          // Apply date filter only if dates are provided

          if(startDate.HasValue && !endDate.HasValue)
          {
            var startingDate = startDate.Value.Date;
            query = query.Where(i => i.DateAdded >= startingDate);
          }

          if(startDate.HasValue && !endDate.HasValue)
          {
            var startingDate = startDate.Value.Date;
            query = query.Where(i => i.DateAdded.Date >= startingDate);
          }

          if (startDate.HasValue && endDate.HasValue)
          {
            var startDateUtc = startDate.Value.Date;
            var endDateUtc = endDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(i => i.DateAdded >= startDateUtc && i.DateAdded <= endDateUtc);
          }

          // Apply status filters
          switch (filterType?.ToLower() ?? "all")
          {
            case "critical":
            case "critical stock":
              query = query.Where(i => i.IsActive && i.BoxQuantity <= i.ReorderLevel);
              break;

            case "near expiry":
            case "nearexpiry":
              query = query.Where(i => i.IsActive &&
                  i.ExpirationDate > now &&
                  i.ExpirationDate <= now.AddDays(90));
              break;

            case "expired":
              query = query.Where(i => i.IsActive &&
                  i.ExpirationDate <= now);
              break;

            case "all":
            case "all inventory":
            default:
              query = query.Where(i => i.IsActive);
              break;
          }

          return await query
            .OrderBy(i => i.Product.ProductName)
            .ThenBy(i => i.ExpirationDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<FastMovingProductDataDto>> GetFastMovingProductsDataAsync(
            DateTime? startDate,
            DateTime? endDate,
            int topN)
        {
          // Step 1: Get sales data (with optional date filter)
          var salesQuery = _context.SalesItem
            .Include(si => si.Product)
            .ThenInclude(p => p.Category)
            .Include(si => si.SalesTransaction)
            .AsQueryable();

          // Apply date filter only if dates are provided

          if(startDate.HasValue && !endDate.HasValue)
          {
            var startingDate = startDate.Value.Date;
            salesQuery = salesQuery.Where(si => si.SalesTransaction.TransactionDate.Date >= startingDate);
          }

          if (startDate.HasValue && endDate.HasValue)
          {
            var startDateUtc = startDate.Value.Date;
            var endDateUtc = endDate.Value.Date.AddDays(1).AddTicks(-1);
            salesQuery = salesQuery.Where(si =>
                si.SalesTransaction.TransactionDate >= startDateUtc &&
                si.SalesTransaction.TransactionDate <= endDateUtc);
          }

          var salesData = await salesQuery
            .GroupBy(si => new
                {
                si.ProductId,
                si.Product.ProductName,
                CategoryName = si.Product.Category.CategoryName
                })
          .Select(g => new
              {
              g.Key.ProductId,
              g.Key.ProductName,
              g.Key.CategoryName,
              QuantitySold = g.Sum(si => si.Quantity),
              TransactionCount = g.Select(si => si.SalesTransactionId).Distinct().Count(),
              Revenue = g.Sum(si => si.Subtotal)
              })
          .OrderByDescending(p => p.QuantitySold)
            .ThenByDescending(p => p.TransactionCount)
            .Take(topN)
            .ToListAsync(); // Materialize here to avoid complex translation

          // Step 2: Get product IDs
          var productIds = salesData.Select(s => s.ProductId).ToList();

          // Step 3: Fetch inventory data using ONLY database columns
          var now = DateTime.UtcNow;

          var inventoryData = await _context.Inventory
            .Where(i => productIds.Contains(i.ProductID)
                && i.IsActive
                && i.ExpirationDate >= now) // Using actual DB column
            .Select(i => new
                {
                i.ProductID,
                i.BoxQuantity,
                i.SheetQuantity,
                i.PieceQuantity,
                i.SheetsPerBox,
                i.PiecesPerSheet,
                i.PiecePerBox
                })
          .ToListAsync(); // Materialize before calculating TotalPiecesAvailable

          // Step 4: Calculate total pieces in memory (client-side)
          var stockByProduct = inventoryData
            .GroupBy(i => i.ProductID)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(i => CalculateTotalPieces(
                    i.BoxQuantity,
                    i.SheetQuantity,
                    i.PieceQuantity,
                    i.SheetsPerBox,
                    i.PiecesPerSheet,
                    i.PiecePerBox
                    ))
                );

          // Step 5: Combine results
          var result = salesData.Select(s => new FastMovingProductDataDto
              {
              ProductId = s.ProductId,
              ProductName = s.ProductName,
              CategoryName = s.CategoryName,
              QuantitySold = s.QuantitySold,
              TransactionCount = s.TransactionCount,
              Revenue = s.Revenue,
              CurrentStock = stockByProduct.ContainsKey(s.ProductId)
              ? stockByProduct[s.ProductId]
              : 0
              }).ToList();

          return result;
        }

        // Helper method to calculate total pieces (same logic as your Inventory model)
        private int CalculateTotalPieces(
            int boxQuantity,
            int sheetQuantity,
            int pieceQuantity,
            int? sheetsPerBox,
            int? piecesPerSheet,
            int? piecePerBox)
        {
          // Case 1: Multi-level (Box → Sheet → Piece)
          if (sheetsPerBox.HasValue && piecesPerSheet.HasValue && piecesPerSheet.Value > 0)
          {
            return (boxQuantity * sheetsPerBox.Value * piecesPerSheet.Value)
              + (sheetQuantity * piecesPerSheet.Value)
              + pieceQuantity;
          }
          // Case 2: Dual-level (Box → Piece)
          else if (piecePerBox.HasValue && (!piecesPerSheet.HasValue || piecesPerSheet.Value == 0))
          {
            return (boxQuantity * piecePerBox.Value) + pieceQuantity;
          }
          // Case 3: Single-level (Only pieces)
          else
          {
            return pieceQuantity;
          }
        }

        #endregion

        #region AdminDashboard
        public async Task<decimal> GetTodaySalesAmountAsync()
        {
            // Get today's date in your local timezone (Philippines = UTC+8)
            var localNow = DateTime.UtcNow.AddHours(8);
            var todayStart = localNow.Date; // Start of today: 00:00:00
            var todayEnd = todayStart.AddDays(1); // Start of tomorrow: 00:00:00

            return await _context.SalesTransaction
                .Where(st => st.TransactionDate >= todayStart
                          && st.TransactionDate < todayEnd)
                .SumAsync(st => st.NetAmount);
        }
        public async Task<int> GetInventoryLowStockCountAsync()
        {
          return await _context.Inventory
            .Where(iv => iv.BoxQuantity <= iv.ReorderLevel)
            .CountAsync();
        }
        public async Task<int> GetInventoryOutOfStockCountAsync()
        {
          return await _context.Inventory
            .Where(iv => iv.BoxQuantity == 0)
            .CountAsync();
        }
        public async Task<List<ProductSalesSummaryDto>> GetTopSellingProductsMonthlyAsync()
        {
          var currentMonth = DateTime.Today.Month;
          var topSellingProducts = await _context.SalesItem
            .Include(si => si.SalesTransaction)
            .Include(si => si.Product)
            .ThenInclude(p => p.Inventory)
            .Where(si => si.SalesTransaction.TransactionDate.Month == currentMonth)
            .GroupBy(si => new {si.Product.ProductID, si.Product.ProductName})
            .Select(g => new
                {
                g.Key.ProductID,
                g.Key.ProductName,
                QuantitySold = g.Sum(si => si.Quantity),
                TotalSales = g.Sum(si => si.Subtotal),
                g.FirstOrDefault().Product
                }).ToListAsync();
          var result = topSellingProducts.Select(ts => new ProductSalesSummaryDto
              {
              ProductId = ts.ProductID,
              ProductName = ts.ProductName,
              QuantitySold = ts.QuantitySold,
              AvailableStock = ts.Product.Inventory
              .Where(i => i.IsActive && !i.IsExpired)
              .Sum(i => i.TotalPiecesAvailable),
              TotalSales = ts.TotalSales
              }).ToList();
          return result;
        }
        #endregion

        #region BackUp Database

        public async Task<BackupListResponse> GetAllBackupsAsync()
        {
          try
          {
            var backupDir = GetBackupDirectory();
            if (!Directory.Exists(backupDir))
            {
              Directory.CreateDirectory(backupDir);
              return new BackupListResponse
              {
                Success = true,
                Backups = new List<BackupDto>(),
                TotalCount = 0,
                TotalSizeMB = "0.00 MB"
              };
            }

            var backupFiles = Directory.GetFiles(backupDir, "*.backup")
              .Select(file => new FileInfo(file))
              .OrderByDescending(f => f.CreationTime)
              .ToList();

            var backups = backupFiles.Select(file => new BackupDto
                {
                FileName = file.Name,
                CreatedDate = file.CreationTime,
                FileSizeBytes = file.Length,
                BackupType = file.Name.Contains("_AUTO_") ? "Automated" : "Manual",
                Status = "Success"
                }).ToList();

            var totalSize = backupFiles.Sum(f => f.Length);

            return new BackupListResponse
            {
              Success = true,
              Backups = backups,
              TotalCount = backups.Count,
              TotalSizeMB = $"{totalSize / (1024.0 * 1024.0):F2} MB"
            };
          }
          catch (Exception ex)
          {
            return new BackupListResponse
            {
              Success = false,
              Backups = new List<BackupDto>(),
              TotalCount = 0,
              TotalSizeMB = "0.00 MB"
            };
          }
        }

        public async Task<byte[]?> DownloadBackupAsync(string fileName)
        {
          try
          {
            var backupDir = GetBackupDirectory();
            var filePath = Path.Combine(backupDir, fileName);

            if (!File.Exists(filePath))
            {
              return null;
            }

            return await File.ReadAllBytesAsync(filePath);
          }
          catch
          {
            return null;
          }
        }

        public string GetBackupDirectory()
        {
          // Get the backup directory from configuration or use default
          var backupPath = _config.GetValue<string>("BackupSettings:BackupPath");

          if (string.IsNullOrEmpty(backupPath))
          {
            // Default to a Backups folder in the application directory
            backupPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
          }

          return backupPath;
        }

        #endregion

        #region StockHistory 
        public async Task<IEnumerable<StockMovement>> GetStockHistoryAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? searchTerm)
        {
            var query = _context.StockMovement
              .Include(sm => sm.Inventory)
              .ThenInclude(i => i.Product)
              .Include(sm => sm.Employee)
              .AsQueryable();
            // Apply date filter
            if (startDate.HasValue && endDate.HasValue)
            {
                var startDateUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc).AddHours(-8);
                var endDateUtc = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc).AddHours(-8);
                query = query.Where(sm => sm.MovementDate >= startDateUtc && sm.MovementDate <= endDateUtc);
            }
            else if (startDate.HasValue)
            {
                var startDateUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc).AddHours(-8);
                query = query.Where(sm => sm.MovementDate >= startDateUtc);
            }
            if (!string.IsNullOrEmpty(transactionType) &&
                !transactionType.Equals("All Transaction", StringComparison.OrdinalIgnoreCase))
            {
                var movementType = transactionType switch
                {
                    "Stock Entry" => "Stock In",
                    "Adjustment" => "Adjustment",
                    "Count Correction" => "Count Correction",
                    "Sale" => "Sale",
                    "Damaged" => "Damaged",
                    "Expired" => "Expired",
                    _ => transactionType
                };
                query = query.Where(sm => sm.MovementType.ToLower() == movementType.ToLower());
            }
            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(sm =>
                    sm.Inventory.Product.ProductName.ToLower().Contains(lowerSearchTerm) ||
                    sm.Inventory.BatchNumber.ToLower().Contains(lowerSearchTerm) ||
                    (sm.ReferenceNumber != null && sm.ReferenceNumber.ToLower().Contains(lowerSearchTerm))
                    );
            }
            return await query
              .OrderByDescending(sm => sm.MovementDate)
              .ToListAsync();
        }
        public async Task<StockHistorySummaryDto> GetStockHistorySummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType)
        {
            var query = _context.StockMovement.AsQueryable();
            // Apply date filter
            if (startDate.HasValue && endDate.HasValue)
            {
                var startDateUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc).AddHours(-8);
                var endDateUtc = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc).AddHours(-8);
                query = query.Where(sm => sm.MovementDate >= startDateUtc && sm.MovementDate <= endDateUtc);
            }
            else if (startDate.HasValue)
            {
                var startDateUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc).AddHours(-8);
                query = query.Where(sm => sm.MovementDate >= startDateUtc);
            }
            // Apply transaction type filter if not "All Transaction"
            if (!string.IsNullOrEmpty(transactionType) &&
                !transactionType.Equals("All Transaction", StringComparison.OrdinalIgnoreCase))
            {
                var movementType = transactionType switch
                {
                    "Stock Entry" => "Stock In",
                    "Adjustment" => "Adjustment",
                    "Sale" => "Sale",
                    "Count Correction" => "Count Correction",
                    "Damaged" => "Damaged",
                    "Expired" => "Expired",
                    _ => transactionType
                };
                query = query.Where(sm => sm.MovementType.ToLower() == movementType.ToLower());
            }
            var movements = await query.ToListAsync();
            return new StockHistorySummaryDto
            {
                TotalTransactions = movements.Count,
                StockEntries = movements.Count(sm => sm.MovementType.ToLower() == "stock in"),
                Adjustments = movements.Count(sm => sm.MovementType.ToLower() == "adjustment" || sm.MovementType.ToLower() == "count correction"),
                SalesRemovals = movements.Count(sm =>
                    sm.MovementType.ToLower() == "sale" ||
                    sm.MovementType.ToLower() == "damaged" ||
                    sm.MovementType.ToLower() == "expired" ||
                    sm.MovementType.ToLower() == "loss"
                  )
            };
        }
        #endregion
    }
}
