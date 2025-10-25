using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.DTOs.DashboardDTOs;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.ReportsDTOs;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.Repository.GET;

namespace PharmacyBackend.Service.GetService
{
    public class GetService : IGetServiceContracts
    {
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly IConfiguration _config;
        private readonly ILogger<GetService> _logger;

        public GetService(IGetRepositoryContracts getRepo, 
            IUpdateRepositoryContracts updateRepo,
            IConfiguration config,
            ILogger<GetService> logger)
        {
            _getRepo = getRepo;
            _updateRepo = updateRepo;
            _config = config;
            _logger = logger;
        }

        #region DashBoardInventoryMan

        public async Task<InventoryDashboardDto> GetInventoryDashboardAsync()
        {
            var dashboard = new InventoryDashboardDto();

            var allProducts = await GetAllProductAsync();
            var allInventory = await GetAllInventoryAsync();
            var criticalStock = await GetCriticalStockInventoryAsync();
            var nearExpiry = await GetNearExpiryInventoryAsync();
            var expiredStock = await GetExpiredInventoryAsync();

            // Calculate summary metrics
            dashboard.TotalActiveProducts = allProducts.Count(p => p.IsActive);
            dashboard.TotalInventoryItems = allInventory.Count(i => i.IsActive);
            dashboard.CriticalStockCount = criticalStock.Count();
            dashboard.NearExpiryCount = nearExpiry.Count();
            dashboard.ExpiredStockCount = expiredStock.Count();

            // Populate detailed lists (limited to first 10 for performance)
            dashboard.CriticalStockItems = criticalStock.Take(5).ToList();
            dashboard.NearExpiryItems = nearExpiry.Take(5).ToList();
            dashboard.ExpiredStockItems = expiredStock.Take(5).ToList();

            return dashboard;
        }
        #endregion

        #region Employees
        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _getRepo.GetAllEmployeesAsync();
            if (!employees.Any() || employees.Count() == 0)
                return new List<EmployeeDTO>();

            var modifEmployees = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                LastName = e.LastName,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                Age = e.Age,
                BirthDate = e.BirthDate,
                CivilStatus = e.CivilStatus,
                Gender = e.Gender,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                FullName = e.FullName,
                RolePosition = e.Role.RolePosition,
                Account = new AccountDTO
                {
                    Id = e.Account.Id,
                    EmployeeId = e.Account.EmployeeId,
                    Username = e.Account.Username,
                    Password = e.Account.EncryptedPassword
                }
            }).ToList();
            return modifEmployees;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _getRepo.GetEmployeeByIdAsync(id);
            if (employee is null)
                return null;
            var modifEmployee = new EmployeeDTO
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
                BirthDate = employee.BirthDate,
                CivilStatus = employee.CivilStatus,
                Gender = employee.Gender,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                FullName = employee.FullName,
                RolePosition = employee.Role.RolePosition,
                Account = new AccountDTO
                {
                    Id = employee.Account.Id,
                    EmployeeId = employee.Account.EmployeeId,
                    Username = employee.Account.Username,
                    Password = employee.Account.EncryptedPassword
                }
            };
            return modifEmployee;
        }

        public async Task<List<EmployeeDTO>> GetEmployeeByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                return null;
            var employee = await _getRepo.GetEmployeeByLastName(lastName);
            if (employee is null)
                return null;
            var modifEmployees = employee.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                LastName = e.LastName,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                Age = e.Age,
                BirthDate = e.BirthDate,
                CivilStatus = e.CivilStatus,
                Gender = e.Gender,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                RolePosition = e.Role.RolePosition,
                FullName = e.FullName
            }).ToList();
            return modifEmployees;
        }

        public async Task<List<EmployeeDTO>> GetFirstDisplayEmployees()
        {
            var employees = await _getRepo.GetFirstDisplayEmployees();
            if (!employees.Any() || employees.Count() == 0)
                return new List<EmployeeDTO>();
            var modifEmployees = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                LastName = e.LastName,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                Age = e.Age,
                BirthDate = e.BirthDate,
                CivilStatus = e.CivilStatus,
                Gender = e.Gender,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                RolePosition = e.Role.RolePosition,
                FullName = e.FullName
            }).ToList();
            return modifEmployees;
        }
        #endregion

        #region EmployeeHistory
        public async Task<List<EmployeeHistoryDTO>> GetAllEmployeeHistoryAsync()
        {
            var employeeHistories = await _getRepo.GetAllEmployeeHistoryAsync();
            if (!employeeHistories.Any() || employeeHistories.Count() == 0)
                return new List<EmployeeHistoryDTO>();
            var modifHistories = employeeHistories.Select(e => new EmployeeHistoryDTO
            {
                EmployeeChanged = e.Employee.FullName,
                ActionPerformed = e.ActionPerformed,
                PerformedBy = e.PerformedByEmployee.FullName,
                PerformedAt = e.PerformedAt
            }).ToList();
            return modifHistories;
        }
        public async Task<EmployeeHistoryDTO> GetEmployeeHistoryByIdAsync(int employeeHistoryId)
        {
            var employeeHistory = await _getRepo.GetEmployeeHistoryByIdAsync(employeeHistoryId);
            if (employeeHistory is null)
                return null;
            var modifHistory = new EmployeeHistoryDTO()
            {
                EmployeeChanged = employeeHistory.Employee.FullName,
                ActionPerformed = employeeHistory.ActionPerformed,
                PerformedBy = employeeHistory.PerformedByEmployee.FullName,
                PerformedAt = employeeHistory.PerformedAt
            };
            return modifHistory;
        }
        public async Task<List<EmployeeHistoryDTO>> GetDisplayEmployeeHistoryAsync()
        {
            var employeeHistories = await _getRepo.GetDisplayEmployeeHistoryAsync();
            if (!employeeHistories.Any() || employeeHistories.Count() == 0)
                return new List<EmployeeHistoryDTO>();

            var modifHistories = employeeHistories.Select(e => new EmployeeHistoryDTO
            {
                EmployeeChanged = e.Employee.FullName,
                ActionPerformed = e.ActionPerformed,
                PerformedBy = e.PerformedByEmployee.FullName,
                PerformedAt = e.PerformedAt
            }).ToList();

            return modifHistories;
        }
        #endregion

        #region Account
        public async Task<bool> AccountUsernameValidation(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;
            var validateUsername = await _getRepo.AccountUsernameValidation(userName);
            return validateUsername;
        }

        public async Task<List<AccountDTO>> GetAllAccountsAsync()
        {
            var accounts = await _getRepo.GetAllAccountsAsync();
            if (!accounts.Any() && accounts.Count() == 0)
                return new List<AccountDTO>();
            var modifAccounts = accounts.Select(a => new AccountDTO
            {
                Id = a.Id,
                Username = a.Username,
                Password = a.EncryptedPassword,
                EmployeeId = a.EmployeeId
            }).ToList();
            return modifAccounts;
        }

        public async Task<AccountDTO> GetAccountByIdAsync(int id)
        {
            var account = await _getRepo.GetAccountByIdAsync(id);
            if (account is null)
                return null;
            var modifAccount = new AccountDTO
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.EncryptedPassword,
                EmployeeId = account.EmployeeId
            };
            return modifAccount;
        }

        public async Task<AccountDTO> SearchAccountByUsernameAsync(string userName)
        {
            var account = await _getRepo.SearchAccountByUsernameAsync(userName);
            if (account is null)
                return null;
            var modifAccount = new AccountDTO
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.EncryptedPassword,
                EmployeeId = account.EmployeeId
            };
            return modifAccount;
        }


        #endregion

        #region Daily Sales
        public async Task<DailySalesSummaryDto> GetSummarySalesForToday()
        {
            var transaction = await _getRepo.GetSummarySalesForToday();
            if (transaction is null)
                return new DailySalesSummaryDto();
            return transaction;
        }

        public async Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? employeeId = null,
            string paymentMethod = null)
        {
            var summary = await _getRepo.GetDailySalesSummaryAsync(
                startDate,
                endDate,
                employeeId,
                paymentMethod
            );

            return summary;
        }

        public async Task<IEnumerable<ProductSalesSummaryDto>> GetProductSalesSummaryAsync(
            DateTime startDate,
            DateTime endDate)
        {
            var products = await _getRepo.GetProductSalesSummaryAsync(startDate, endDate);

            return products;
        }

        public async Task<IEnumerable<SalesTransactionDto>> GetFilteredSalesTransactionsAsync(
            DateTime startDate,
            DateTime endDate,
            int? employeeId = null,
            string paymentMethod = null)
        {
            var transactions = await _getRepo.GetFilteredSalesTransactionsAsync(
                startDate,
                endDate,
                employeeId,
                paymentMethod
            );

            // Map to DTO
            var transactionDtos = transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    SalesTransactionId = i.SalesTransactionId,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            });

            return transactionDtos;
        }

        #endregion

        #region Sales

        public async Task<SalesTransactionDto?> GetSalesTransactionByIdAsync(int id)
        {
            var transaction = await _getRepo.GetSalesTransactionByIdAsync(id);
            if (transaction == null)
                return null;

            return new SalesTransactionDto
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                InvoiceNumber = transaction.InvoiceNumber,
                TotalAmount = transaction.TotalAmount,
                DiscountAmount = transaction.DiscountAmount,
                VATAmount = transaction.VATAmount,
                NetAmount = transaction.NetAmount,
                PaymentMethod = transaction.PaymentMethod,
                AmountPaid = transaction.AmountPaid,
                ChangeAmount = transaction.ChangeAmount,
                DiscountId = transaction.DiscountId,
                EmployeeID = transaction.EmployeeID,
                VATId = transaction.VATId,
                DiscountName = transaction.Discount?.DiscountName ?? string.Empty,
                EmployeeName = transaction.Employee != null ? $"{transaction.Employee.FirstName} {transaction.Employee.LastName}" : string.Empty,
                VATRate = transaction.VAT?.VATRate,
                Items = transaction.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    SalesTransactionId = i.SalesTransactionId,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        public async Task<IEnumerable<SalesTransactionDto>> GetAllSalesTransactionsAsync()
        {
            var transactions = await _getRepo.GetAllSalesTransactionsAsync();
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            });
        }

        public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByEmployeeIdAsync(int employeeId)
        {
            var transactions = await _getRepo.GetSalesTransactionsByEmployeeIdAsync(employeeId);
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            });
        }

        public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _getRepo.GetSalesTransactionsByDateRangeAsync(startDate, endDate);
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            });
        }

        public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber)
        {
            var transactions = await _getRepo.GetSalesTransactionsByInvoiceNumberAsync(invoiceNumber);
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            });
        }

        public async Task<SalesItemDto?> GetSalesItemByIdAsync(int id)
        {
            var item = await _getRepo.GetSalesItemByIdAsync(id);
            if (item == null)
                return null;

            return new SalesItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.ProductName ?? string.Empty,
                UnitType = item.UnitType,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Subtotal = item.Subtotal
            };
        }

        public async Task<IEnumerable<SalesItemDto>> GetSalesItemsByTransactionIdAsync(int transactionId)
        {
            var items = await _getRepo.GetSalesItemsByTransactionIdAsync(transactionId);
            return items.Select(i => new SalesItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.ProductName ?? string.Empty,
                UnitType = i.UnitType,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Subtotal
            });
        }

        #endregion

        #region Discount
        public async Task<IEnumerable<DiscountDto>> GetAllDiscountAsync()
        {
            var discounts = await _getRepo.GetAllDiscountAsync();
            return discounts.Select(d => new DiscountDto
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                DiscountRate = d.DiscountRate,
                IsActive = d.IsActive
            });
        }

        public async Task<DiscountDto?> GetDiscountByIdAsync(int id)
        {
            var d = await _getRepo.GetDiscountByIdAsync(id);
            if (d == null)
                return null;
            return new DiscountDto
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                DiscountRate = d.DiscountRate,
                IsActive = d.IsActive
            };
        }
        #endregion region

        #region Products
        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            var products = await _getRepo.GetAllProductAsync();
            return products.Select(p => new ProductDto
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                CategoryID = p.CategoryID,
                BrandID = p.BrandID,
                GenericID = p.GenericID,
                FormulationID = p.FormulationID,
                ClassificationID = p.ClassificationID,
                TypeID = p.TypeID,
                IsActive = p.IsActive,
                DateCreated = p.DateCreated,
                SheetsPerBox = p.SheetsPerBox,
                PiecesPerSheet = p.PiecesPerSheet,
                BoxPrice = p.BoxPrice,
                SheetPrice = p.SheetPrice,
                PiecePrice = p.PiecePrice,
                ReorderLevel = p.ReorderLevel,
                UnitMeasurement = p.UnitMeasurement,
                Description = p.Description,
                PiecePerBox = p.PiecePerBox,


                // Inventory calculations
                //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
                HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

                // Related entity names
                CategoryName = p.Category?.CategoryName ?? string.Empty,
                BrandName = p.Brand?.BrandName ?? string.Empty,
                GenericName = p.Generic?.GenericName ?? string.Empty,
                FormulationName = p.Formulation?.FormulationName ?? string.Empty,
                ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
                TypeName = p.TypeModel?.TypeName ?? string.Empty,
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var p = await _getRepo.GetProductByIdAsync(id);
            if (p == null)
                return null;

            return new ProductDto
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                CategoryID = p.CategoryID,
                BrandID = p.BrandID,
                GenericID = p.GenericID,
                FormulationID = p.FormulationID,
                ClassificationID = p.ClassificationID,
                TypeID = p.TypeID,
                IsActive = p.IsActive,
                DateCreated = p.DateCreated,
                SheetsPerBox = p.SheetsPerBox,
                PiecesPerSheet = p.PiecesPerSheet,
                BoxPrice = p.BoxPrice,
                SheetPrice = p.SheetPrice,
                PiecePrice = p.PiecePrice,
                ReorderLevel = p.ReorderLevel,
                UnitMeasurement = p.UnitMeasurement,
                Description = p.Description,
                PiecePerBox = p.PiecePerBox,

                // Inventory calculations
                //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
                IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
                HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

                // Related entity names
                CategoryName = p.Category?.CategoryName ?? string.Empty,
                BrandName = p.Brand?.BrandName ?? string.Empty,
                GenericName = p.Generic?.GenericName ?? string.Empty,
                FormulationName = p.Formulation?.FormulationName ?? string.Empty,
                ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
                TypeName = p.TypeModel?.TypeName ?? string.Empty,
            };
        }
        #endregion

        #region Category
        public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync()
        {
            var categories = await _getRepo.GetAllCategoryAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var c = await _getRepo.GetCategoryByIdAsync(id);
            if (c == null)
                return null;
            return new CategoryDto
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            };
        }
        #endregion

        #region Brand
        public async Task<IEnumerable<BrandDto>> GetAllBrandAsync()
        {
            var brand = await _getRepo.GetAllBrandAsync();
            return brand.Select(b => new BrandDto
            {
                BrandID = b.BrandID,
                BrandName = b.BrandName
            });
        }

        public async Task<BrandDto?> GetBrandByIdAsync(int id)
        {
            var b = await _getRepo.GetBrandByIdAsync(id);
            if (b == null)
                return null;
            return new BrandDto
            {
                BrandID = b.BrandID,
                BrandName = b.BrandName
            };
        }
        #endregion

        #region Generic
        public async Task<IEnumerable<GenericDto>> GetAllGenericAsync()
        {
            var generic = await _getRepo.GetAllGenericAsync();
            return generic.Select(g => new GenericDto
            {
                GenericID = g.GenericID,
                GenericName = g.GenericName
            });
        }

        public async Task<GenericDto?> GetGenericByIdAsync(int id)
        {
            var g = await _getRepo.GetGenericByIdAsync(id);
            if (g == null)
                return null;
            return new GenericDto
            {
                GenericID = g.GenericID,
                GenericName = g.GenericName
            };
        }
        #endregion

        #region Formulation
        public async Task<IEnumerable<FormulationDto>> GetAllFormulationAsync()
        {
            var Formulation = await _getRepo.GetAllFormulationAsync();
            return Formulation.Select(f => new FormulationDto
            {
                FormulationID = f.FormulationID,
                FormulationName = f.FormulationName
            });
        }

        public async Task<FormulationDto?> GetFormulationByIdAsync(int id)
        {
            var f = await _getRepo.GetFormulationByIdAsync(id);
            if (f == null)
                return null;
            return new FormulationDto
            {
                FormulationID = f.FormulationID,
                FormulationName = f.FormulationName
            };
        }
        #endregion

        #region Classification
        public async Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync()
        {
            var classification = await _getRepo.GetAllClassificationAsync();
            return classification.Select(c => new ClassificationDto
            {
                ClassificationID = c.ClassificationID,
                ClassificationName = c.ClassificationName
            });
        }

        public async Task<ClassificationDto?> GetClassificationByIdAsync(int id)
        {
            var c = await _getRepo.GetClassificationByIdAsync(id);
            if (c == null)
                return null;
            return new ClassificationDto
            {
                ClassificationID = c.ClassificationID,
                ClassificationName = c.ClassificationName
            };
        }
        #endregion

        #region Type
        public async Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync()
        {
            var type = await _getRepo.GetAllTypeModelAsync();
            return type.Select(t => new TypeModelDto
            {
                TypeID = t.TypeID,
                TypeName = t.TypeName
            });
        }

        public async Task<TypeModelDto?> GetTypeModelByIdAsync(int id)
        {
            var t = await _getRepo.GetTypeModelByIdAsync(id);
            if (t == null)
                return null;
            return new TypeModelDto
            {
                TypeID = t.TypeID,
                TypeName = t.TypeName
            };
        }
        #endregion

        #region Supplier
        public async Task<IEnumerable<SupplierDTO>> GetAllSupplierAsync()
        {
            var suppliers = await _getRepo.GetAllSupplierAsync();
            return suppliers.Select(s => new SupplierDTO
            {
                Id = s.Id,
                SupplierName = s.SupplierName,
                ContactNumber = s.ContactNumber,
                ContactPerson = s.ContactPerson,
                ContactPersonNumber = s.ContactPersonNumber,
                Email = s.Email,
                Address = s.Address,
                IsActive = s.IsActive,
                Products = s.Products.Select(p => new ProductDto
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryID = p.CategoryID,
                    BrandID = p.BrandID,
                    GenericID = p.GenericID,
                    FormulationID = p.FormulationID,
                    ClassificationID = p.ClassificationID,
                    TypeID = p.TypeID,
                    IsActive = p.IsActive,
                    DateCreated = p.DateCreated,
                    SheetsPerBox = p.SheetsPerBox,
                    PiecesPerSheet = p.PiecesPerSheet,
                    BoxPrice = p.BoxPrice,
                    SheetPrice = p.SheetPrice,
                    PiecePrice = p.PiecePrice,
                    ReorderLevel = p.ReorderLevel,
                    UnitMeasurement = p.UnitMeasurement,
                    PiecePerBox = p.PiecePerBox,

                    // Inventory calculations
                    HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

                    // Related entity names
                    CategoryName = p.Category?.CategoryName ?? string.Empty,
                    BrandName = p.Brand?.BrandName ?? string.Empty,
                    GenericName = p.Generic?.GenericName ?? string.Empty,
                    FormulationName = p.Formulation?.FormulationName ?? string.Empty,
                    ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
                    TypeName = p.TypeModel?.TypeName ?? string.Empty,
                }).ToList()
            });
        }

        public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
        {
            var s = await _getRepo.GetSupplierByIdAsync(id);
            if (s == null)
                return null;
            return new SupplierDTO
            {
                Id = s.Id,
                SupplierName = s.SupplierName,
                ContactNumber = s.ContactNumber,
                ContactPerson = s.ContactPerson,
                ContactPersonNumber = s.ContactPersonNumber,
                Email = s.Email,
                Address = s.Address,
                IsActive = s.IsActive
            };
        }
        #endregion

        #region Inventory 
        public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
        {
            // Get inventories from repository (ensure repo returns Product navigation if possible)
            var inventories = await _getRepo.GetAllInventoryAsync();

            // Deactivate inventories with invalid total pieces (existing behavior)
            foreach (var inv in inventories)
            {
                if (inv.TotalPiecesAvailable < 0)
                {
                    inv.IsActive = false;
                    await _updateRepo.UpdateInventoryAsync(inv);
                }
            }

            // Map to DTOs (only fields present on InventoryDto)
            var result = inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,

                // Map nested Product -> ProductDto (safe null checks)
                Product = i.Product == null ? null : new ProductDto
                {
                    ProductID = i.Product.ProductID,
                    ProductName = i.Product.ProductName,
                    CategoryID = i.Product.CategoryID,
                    BrandID = i.Product.BrandID,
                    GenericID = i.Product.GenericID,
                    FormulationID = i.Product.FormulationID,
                    ClassificationID = i.Product.ClassificationID,
                    TypeID = i.Product.TypeID,
                    IsActive = i.Product.IsActive,
                    DateCreated = i.Product.DateCreated,
                    SheetsPerBox = i.Product.SheetsPerBox,
                    PiecesPerSheet = i.Product.PiecesPerSheet,
                    BoxPrice = i.Product.BoxPrice,
                    SheetPrice = i.Product.SheetPrice,
                    PiecePrice = i.Product.PiecePrice,
                    ReorderLevel = i.Product.ReorderLevel,
                    UnitMeasurement = i.Product.UnitMeasurement,
                    PiecePerBox = i.PiecePerBox,

                    // Derived product properties (guarded against null Inventory collections)
                    ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
                    NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
                    IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
                    HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

                    // Related names
                    CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
                    BrandName = i.Product.Brand?.BrandName ?? string.Empty,
                    GenericName = i.Product.Generic?.GenericName ?? string.Empty,
                    FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
                    ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
                    TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
                },

                // Inventory-specific fields
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,

                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,

                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,

                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,

                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,

                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            })
            .ToList();

            return result;
        }
        public async Task<List<InventoryDto>> GetAllAvailableInventoryAsync()
        {
            var inventories = await _getRepo.GetAllAvailableInventoryAsync();
            if (inventories.Count() == 0)
                return new List<InventoryDto>();
            return inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                PiecePerBox = i.PiecePerBox,

                // Map nested Product -> ProductDto (safe null checks)
                Product = i.Product == null ? null : new ProductDto
                {
                    ProductID = i.Product.ProductID,
                    ProductName = i.Product.ProductName,
                    CategoryID = i.Product.CategoryID,
                    BrandID = i.Product.BrandID,
                    GenericID = i.Product.GenericID,
                    FormulationID = i.Product.FormulationID,
                    ClassificationID = i.Product.ClassificationID,
                    TypeID = i.Product.TypeID,
                    IsActive = i.Product.IsActive,
                    DateCreated = i.Product.DateCreated,
                    SheetsPerBox = i.Product.SheetsPerBox,
                    PiecesPerSheet = i.Product.PiecesPerSheet,
                    BoxPrice = i.Product.BoxPrice,
                    SheetPrice = i.Product.SheetPrice,
                    PiecePrice = i.Product.PiecePrice,
                    ReorderLevel = i.Product.ReorderLevel,
                    UnitMeasurement = i.Product.UnitMeasurement,
                    PiecePerBox = i.Product.PiecePerBox,

                    // Derived product properties (guarded against null Inventory collections)
                    ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
                    NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
                    IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
                    HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

                    // Related names
                    CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
                    BrandName = i.Product.Brand?.BrandName ?? string.Empty,
                    GenericName = i.Product.Generic?.GenericName ?? string.Empty,
                    FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
                    ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
                    TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
                },

                // Inventory-specific fields
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,

                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,

                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,

                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,

                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,

                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            }).ToList();
        }

        public async Task<InventoryDto?> GetInventoryByIdAsync(int id)
        {
            var i = await _getRepo.GetInventoryByIdAsync(id);
            if (i == null)
                return null;
            return new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            };
        }

        public async Task<IEnumerable<InventoryDto>> GetInventoryByProductIdAsync(int productId)
        {
            var inventories = await _getRepo.GetInventoryByProductIdAsync(productId);
            return inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            });
        }

        public async Task<IEnumerable<InventoryDto>> GetExpiredInventoryAsync()
        {
            var inventories = await _getRepo.GetExpiredInventoryAsync();
            return inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            });

        }

        public async Task<IEnumerable<InventoryDto>> GetNearExpiryInventoryAsync()
        {
            var inventories = await _getRepo.GetNearExpiryInventoryAsync();
            return inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            });
        }

        public async Task<IEnumerable<InventoryDto>> GetCriticalStockInventoryAsync()
        {
            var inventories = await _getRepo.GetCriticalStockInventoryAsync();
            return inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable
            });
        }
        #endregion

        #region VAT
        public async Task<IEnumerable<VatDto>> GetAllVATAsync()
        {
            var vats = await _getRepo.GetAllVATAsync();
            return vats.Select(v => new VatDto
            {
                VATID = v.VATID,
                VATName = v.VATName,
                VATRate = v.VATRate,
                IsActive = v.IsActive,
                DateCreated = v.DateCreated
            });
        }

        public async Task<VatDto?> GetVATByIdAsync(int id)
        {
            var v = await _getRepo.GetVATByIdAsync(id);
            if (v == null)
                return null;
            return new VatDto
            {
                VATID = v.VATID,
                VATName = v.VATName,
                VATRate = v.VATRate,
                IsActive = v.IsActive,
                DateCreated = v.DateCreated
            };
        }
        #endregion
            
        #region PurchaseOrder
        public async Task<IEnumerable<PurchaseOrderDTO>> GetAllPurchaseOrderAsync()
        {
            var orders = await _getRepo.GetAllPurchaseOrderAsync();
            return orders.Select(po => new PurchaseOrderDTO
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    Id = poi.Id,
                    ProductId = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    BoxQuantityReceived = poi.BoxQuantityReceived,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    SheetQuantityReceived = poi.SheetQuantityReceived,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    PieceQuantityReceived = poi.PieceQuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    BoxPrice  = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
        }

        public async Task<PurchaseOrderDTO?> GetPurchaseOrderByIdAsync(int id)
        {
            var po = await _getRepo.GetPurchaseOrderByIdAsync(id);
            if (po == null)
                return null;
            return new PurchaseOrderDTO
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    Id = poi.Id,
                    ProductId = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    BoxQuantityReceived = poi.BoxQuantityReceived,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    SheetQuantityReceived = poi.SheetQuantityReceived,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    PieceQuantityReceived = poi.PieceQuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    BoxPrice = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost
                }).ToList()
            };
        }

        public async Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderBySupplierIdAsync(int supplierId)
        {
            var orders = await _getRepo.GetPurchaseOrderBySupplierIdAsync(supplierId);
            return orders.Select(po => new PurchaseOrderDTO
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    Id = poi.Id,
                    ProductId = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    BoxQuantityReceived = poi.BoxQuantityReceived,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    SheetQuantityReceived = poi.SheetQuantityReceived,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    PieceQuantityReceived = poi.PieceQuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    BoxPrice = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
        }

        public async Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderByStatusAsync(string status)
        {
            var orders = await _getRepo.GetPurchaseOrderByStatusAsync(status);
            return orders.Select(po => new PurchaseOrderDTO
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierName = po.Supplier.SupplierName,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    Id = poi.Id,
                    ProductId = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    BoxQuantityReceived = poi.BoxQuantityReceived,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    SheetQuantityReceived = poi.SheetQuantityReceived,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    PieceQuantityReceived = poi.PieceQuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    BoxPrice = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
        }
        public async Task<List<PurchaseOrderDTO>> GetPendingPurchaseOrdersAsync()
        {
            var purchaseOrders = await _getRepo.GetPendingPurchaseOrdersAsync();
            if (!purchaseOrders.Any() || purchaseOrders.Count() == 0)
                return new List<PurchaseOrderDTO>();

            var modifPurchaseOrders = purchaseOrders.Select(po => new PurchaseOrderDTO
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierName = po.Supplier.SupplierName,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    Id = poi.Id,
                    ProductId = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    BoxQuantityReceived = poi.BoxQuantityReceived,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    SheetQuantityReceived = poi.SheetQuantityReceived,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    PieceQuantityReceived = poi.PieceQuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    BoxPrice = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost
                }).ToList()
            }).ToList();
            return modifPurchaseOrders;
        }
        #endregion

        #region StockMovement
        public async Task<IEnumerable<StockMovementDto>> GetAllStockMovementAsync()
        {
            var movements = await _getRepo.GetAllStockMovementAsync();
            return movements.Select(sm => new StockMovementDto
            {
                StockMovementID = sm.StockMovementID,
                InventoryID = sm.InventoryID,
                ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
                BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
                MovementType = sm.MovementType,
                BoxQuantityChanged = sm.BoxQuantityChanged,
                SheetQuantityChanged = sm.SheetQuantityChanged,
                PieceQuantityChanged = sm.PieceQuantityChanged,
                BoxQuantityBefore = sm.BoxQuantityBefore,
                SheetQuantityBefore = sm.SheetQuantityBefore,
                PieceQuantityBefore = sm.PieceQuantityBefore,
                BoxQuantityAfter = sm.BoxQuantityAfter,
                SheetQuantityAfter = sm.SheetQuantityAfter,
                PieceQuantityAfter = sm.PieceQuantityAfter,
                ReferenceNumber = sm.ReferenceNumber,
                Notes = sm.Notes,
                MovementDate = sm.MovementDate,
                EmployeeID = sm.EmployeeID,
                EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
            });
        }

        public async Task<StockMovementDto?> GetStockMovementByIdAsync(int id)
        {
            var sm = await _getRepo.GetStockMovementByIdAsync(id);
            if (sm == null)
                return null;
            return new StockMovementDto
            {
                StockMovementID = sm.StockMovementID,
                InventoryID = sm.InventoryID,
                ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
                BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
                MovementType = sm.MovementType,
                BoxQuantityChanged = sm.BoxQuantityChanged,
                SheetQuantityChanged = sm.SheetQuantityChanged,
                PieceQuantityChanged = sm.PieceQuantityChanged,
                BoxQuantityBefore = sm.BoxQuantityBefore,
                SheetQuantityBefore = sm.SheetQuantityBefore,
                PieceQuantityBefore = sm.PieceQuantityBefore,
                BoxQuantityAfter = sm.BoxQuantityAfter,
                SheetQuantityAfter = sm.SheetQuantityAfter,
                PieceQuantityAfter = sm.PieceQuantityAfter,
                ReferenceNumber = sm.ReferenceNumber,
                Notes = sm.Notes,
                MovementDate = sm.MovementDate,
                EmployeeID = sm.EmployeeID,
                EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
            };
        }

        public async Task<IEnumerable<StockMovementDto>> GetStockMovementByInventoryIdAsync(int inventoryId)
        {
            var movements = await _getRepo.GetStockMovementByInventoryIdAsync(inventoryId);
            return movements.Select(sm => new StockMovementDto
            {
                StockMovementID = sm.StockMovementID,
                InventoryID = sm.InventoryID,
                ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
                BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
                MovementType = sm.MovementType,
                BoxQuantityChanged = sm.BoxQuantityChanged,
                SheetQuantityChanged = sm.SheetQuantityChanged,
                PieceQuantityChanged = sm.PieceQuantityChanged,
                BoxQuantityBefore = sm.BoxQuantityBefore,
                SheetQuantityBefore = sm.SheetQuantityBefore,
                PieceQuantityBefore = sm.PieceQuantityBefore,
                BoxQuantityAfter = sm.BoxQuantityAfter,
                SheetQuantityAfter = sm.SheetQuantityAfter,
                PieceQuantityAfter = sm.PieceQuantityAfter,
                ReferenceNumber = sm.ReferenceNumber,
                Notes = sm.Notes,
                MovementDate = sm.MovementDate,
                EmployeeID = sm.EmployeeID,
                EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
            });
        }

        public async Task<IEnumerable<StockMovementDto>> GetStockMovementByProductIdAsync(int productId)
        {
            var movements = await _getRepo.GetStockMovementByProductIdAsync(productId);
            return movements.Select(sm => new StockMovementDto
            {
                StockMovementID = sm.StockMovementID,
                InventoryID = sm.InventoryID,
                ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
                BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
                MovementType = sm.MovementType,
                BoxQuantityChanged = sm.BoxQuantityChanged,
                SheetQuantityChanged = sm.SheetQuantityChanged,
                PieceQuantityChanged = sm.PieceQuantityChanged,
                BoxQuantityBefore = sm.BoxQuantityBefore,
                SheetQuantityBefore = sm.SheetQuantityBefore,
                PieceQuantityBefore = sm.PieceQuantityBefore,
                BoxQuantityAfter = sm.BoxQuantityAfter,
                SheetQuantityAfter = sm.SheetQuantityAfter,
                PieceQuantityAfter = sm.PieceQuantityAfter,
                ReferenceNumber = sm.ReferenceNumber,
                Notes = sm.Notes,
                MovementDate = sm.MovementDate,
                EmployeeID = sm.EmployeeID,
                EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
            });
        }
        #endregion

        #region Reports

        public async Task<SalesReportDto> GetSalesReportAsync(
             DateTime? startDate = null,
             DateTime? endDate = null,
             int? employeeId = null,
             string paymentMethod = null)
        {
            var transactions = await _getRepo.GetSalesReportDataAsync(
                startDate,
                endDate,
                employeeId,
                paymentMethod);

            var transactionDtos = transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                VATAmount = t.VATAmount,
                NetAmount = t.NetAmount,
                PaymentMethod = t.PaymentMethod,
                AmountPaid = t.AmountPaid,
                ChangeAmount = t.ChangeAmount,
                DiscountId = t.DiscountId,
                EmployeeID = t.EmployeeID,
                VATId = t.VATId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
                VATRate = t.VAT?.VATRate,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    SalesTransactionId = i.SalesTransactionId,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitType = i.UnitType,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            }).ToList();

            var summary = new SalesReportSummaryDto
            {
                TotalTransactions = transactionDtos.Count(),
                NetRevenue = transactionDtos.Sum(t => t.NetAmount),
                TotalItemsSold = transactionDtos.Sum(t => t.Items.Sum(i => i.Quantity)),
                AverageTransaction = transactionDtos.Any()
                    ? transactionDtos.Average(t => t.NetAmount)
                    : 0
            };

            return new SalesReportDto
            {
                Summary = summary,
                Transactions = transactionDtos
            };
        }

        //Inventory-Reports

        public async Task<InventoryReportDto> GetInventoryReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string filterType = "all")
        {
            var inventories = await _getRepo.GetInventoryReportDataAsync(
                startDate,
                endDate,
                filterType);

            var inventoryDtos = inventories.Select(i => new InventoryDto
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                ProductName = i.Product?.ProductName ?? string.Empty,
                BatchNumber = i.BatchNumber,
                ManufacturingDate = i.ManufacturingDate,
                ExpirationDate = i.ExpirationDate,
                BoxQuantity = i.BoxQuantity,
                SheetQuantity = i.SheetQuantity,
                PieceQuantity = i.PieceQuantity,
                SheetsPerBox = i.SheetsPerBox,
                PiecesPerSheet = i.PiecesPerSheet,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                BoxPrice = i.BoxPrice,
                SheetPrice = i.SheetPrice,
                PiecePrice = i.PiecePrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock,
                IsMultiUnit = i.IsMultiUnit,
                TotalPiecesAvailable = i.TotalPiecesAvailable,
                Product = i.Product == null ? null : new ProductDto
                {
                    ProductID = i.Product.ProductID,
                    ProductName = i.Product.ProductName,
                    CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
                    BrandName = i.Product.Brand?.BrandName ?? string.Empty,
                    GenericName = i.Product.Generic?.GenericName ?? string.Empty,
                    FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
                    ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
                    TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
                }
            }).ToList();

            var summary = new InventoryReportSummaryDto
            {
                TotalProducts = inventoryDtos.Select(i => i.ProductID).Distinct().Count(),
                TotalValue = inventoryDtos.Sum(i => i.TotalPiecesAvailable * i.PiecePrice),
                CriticalStockCount = inventoryDtos.Count(i => i.IsCriticalStock),
                NearExpiryCount = inventoryDtos.Count(i => i.IsNearExpiry && !i.IsExpired)
            };

            return new InventoryReportDto
            {
                Summary = summary,
                InventoryItems = inventoryDtos
            };
        }

        //Fast-Moving Products

        public async Task<FastMovingProductReportDto> GetFastMovingProductsReportAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int topN = 20)
        {
            var productsData = await _getRepo.GetFastMovingProductsDataAsync(
                startDate,
                endDate,
                topN);

            var products = productsData.Select((p, index) => new FastMovingProductDto
            {
                Rank = index + 1,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryName = p.CategoryName,
                QuantitySold = p.QuantitySold,
                TransactionCount = p.TransactionCount,
                Revenue = p.Revenue,
                CurrentStock = p.CurrentStock,
                Trend = DetermineTrend(p.QuantitySold, p.CurrentStock)
            }).ToList();

            return new FastMovingProductReportDto
            {
                TotalProductsSold = products.Count,
                TotalQuantitySold = products.Sum(p => p.QuantitySold),
                TotalRevenue = products.Sum(p => p.Revenue),
                Products = products
            };
        }

        private string DetermineTrend(int quantitySold, int currentStock)
        {
            if (currentStock == 0) return "Decreasing";

            var ratio = (double)quantitySold / currentStock;

            if (ratio > 2.0) return "Increasing";
            if (ratio > 0.5) return "Stable";
            return "Decreasing";
        }

        #endregion

        #region Reports Exports In excell

        //SALES REPORTS

        public async Task<byte[]> ExportSalesReportToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? employeeId = null,
            string paymentMethod = null)
        {
            var report = await GetSalesReportAsync(startDate, endDate, employeeId, paymentMethod);

            using (var package = new ExcelPackage())
            {
                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");

                summarySheet.Cells["A1"].Value = "Sales Report Summary";
                summarySheet.Cells["A1:B1"].Merge = true;
                summarySheet.Cells["A1:B1"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B1"].Style.Font.Size = 16;
                summarySheet.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                summarySheet.Cells["A3"].Value = "Period:";
                summarySheet.Cells["B3"].Value = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";

                if (employeeId.HasValue)
                {
                    var employee = await GetEmployeeByIdAsync(employeeId.Value);
                    summarySheet.Cells["A4"].Value = "Employee:";
                    summarySheet.Cells["B4"].Value = employee?.FullName ?? "N/A";
                }

                if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod.ToLower() != "all methods")
                {
                    summarySheet.Cells["A5"].Value = "Payment Method:";
                    summarySheet.Cells["B5"].Value = paymentMethod;
                }

                int summaryRow = 7;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Transactions";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.TotalTransactions;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Net Revenue";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.NetRevenue;
                summarySheet.Cells[$"B{summaryRow}"].Style.Numberformat.Format = "₱#,##0.00";

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Items Sold";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.TotalItemsSold;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Average Transaction";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.AverageTransaction;
                summarySheet.Cells[$"B{summaryRow}"].Style.Numberformat.Format = "₱#,##0.00";

                summarySheet.Cells["A7:A10"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B10"].AutoFitColumns();

                // Transactions Sheet
                var transactionsSheet = package.Workbook.Worksheets.Add("Transactions");

                // Headers
                transactionsSheet.Cells["A1"].Value = "Invoice";
                transactionsSheet.Cells["B1"].Value = "Date";
                transactionsSheet.Cells["C1"].Value = "Employee";
                transactionsSheet.Cells["D1"].Value = "Items";
                transactionsSheet.Cells["E1"].Value = "Total";
                transactionsSheet.Cells["F1"].Value = "Payment";

                // Style headers
                using (var range = transactionsSheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Data
                int row = 2;
                foreach (var transaction in report.Transactions)
                {
                    transactionsSheet.Cells[row, 1].Value = transaction.InvoiceNumber;
                    transactionsSheet.Cells[row, 2].Value = transaction.TransactionDate;
                    transactionsSheet.Cells[row, 2].Style.Numberformat.Format = "yyyy-mm-dd hh:mm AM/PM";
                    transactionsSheet.Cells[row, 3].Value = transaction.EmployeeName;
                    transactionsSheet.Cells[row, 4].Value = transaction.Items.Sum(i => i.Quantity);
                    transactionsSheet.Cells[row, 5].Value = transaction.NetAmount;
                    transactionsSheet.Cells[row, 5].Style.Numberformat.Format = "₱#,##0.00";
                    transactionsSheet.Cells[row, 6].Value = transaction.PaymentMethod;
                    row++;
                }

                // Add borders
                using (var range = transactionsSheet.Cells[1, 1, row - 1, 6])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                transactionsSheet.Cells[transactionsSheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        //INVENTORY REPORTS

        public async Task<byte[]> ExportInventoryReportToExcelAsync(
            DateTime? startDate =null,
            DateTime? endDate = null,
            string filterType = "all")
        {
            var report = await GetInventoryReportAsync(startDate, endDate, filterType);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");

                summarySheet.Cells["A1"].Value = "Inventory Report Summary";
                summarySheet.Cells["A1:B1"].Merge = true;
                summarySheet.Cells["A1:B1"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B1"].Style.Font.Size = 16;
                summarySheet.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                summarySheet.Cells["A3"].Value = "Period:";
                summarySheet.Cells["B3"].Value = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";

                summarySheet.Cells["A4"].Value = "Filter Type:";
                summarySheet.Cells["B4"].Value = filterType;

                int summaryRow = 6;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Products";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.TotalProducts;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Value";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.TotalValue;
                summarySheet.Cells[$"B{summaryRow}"].Style.Numberformat.Format = "₱#,##0.00";

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Critical Stock Count";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.CriticalStockCount;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Near Expiry Count";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.Summary.NearExpiryCount;

                summarySheet.Cells["A6:A9"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B9"].AutoFitColumns();

                // Inventory Details Sheet
                var detailsSheet = package.Workbook.Worksheets.Add("Inventory Details");

                // Headers
                detailsSheet.Cells["A1"].Value = "Product";
                detailsSheet.Cells["B1"].Value = "Category";
                detailsSheet.Cells["C1"].Value = "Batch";
                detailsSheet.Cells["D1"].Value = "Stock";
                detailsSheet.Cells["E1"].Value = "Expiry Date";
                detailsSheet.Cells["F1"].Value = "Status";

                // Style headers
                using (var range = detailsSheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Data
                int row = 2;
                foreach (var inventory in report.InventoryItems)
                {
                    detailsSheet.Cells[row, 1].Value = inventory.ProductName;
                    detailsSheet.Cells[row, 2].Value = inventory.Product?.CategoryName ?? "N/A";
                    detailsSheet.Cells[row, 3].Value = inventory.BatchNumber;

                    // Display stock based on unit type
                    string stockDisplay = "";
                    if (inventory.BoxQuantity > 0)
                        stockDisplay += $"{inventory.BoxQuantity} boxes ";
                    if (inventory.SheetQuantity > 0)
                        stockDisplay += $"{inventory.SheetQuantity} sheets ";
                    if (inventory.PieceQuantity > 0)
                        stockDisplay += $"{inventory.PieceQuantity} pieces";

                    detailsSheet.Cells[row, 4].Value = stockDisplay.Trim();
                    detailsSheet.Cells[row, 5].Value = inventory.ExpirationDate;
                    detailsSheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-mm-dd";

                    // Status
                    string status = "Active";
                    if (inventory.IsExpired)
                    {
                        status = "Expired";
                        detailsSheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Red);
                    }
                    else if (inventory.IsNearExpiry)
                    {
                        status = "Near Expiry";
                        detailsSheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Orange);
                    }
                    else if (inventory.IsCriticalStock)
                    {
                        status = "Critical Stock";
                        detailsSheet.Cells[row, 6].Style.Font.Color.SetColor(Color.DarkOrange);
                    }

                    detailsSheet.Cells[row, 6].Value = status;
                    row++;
                }

                // Add borders
                using (var range = detailsSheet.Cells[1, 1, row - 1, 6])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                detailsSheet.Cells[detailsSheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public async Task<byte[]> ExportFastMovingProductsToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int topN = 20)
        {
            var report = await GetFastMovingProductsReportAsync(startDate, endDate, topN);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Summary Sheet
                var summarySheet = package.Workbook.Worksheets.Add("Summary");

                summarySheet.Cells["A1"].Value = "Fast-Moving Products Report";
                summarySheet.Cells["A1:B1"].Merge = true;
                summarySheet.Cells["A1:B1"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B1"].Style.Font.Size = 16;
                summarySheet.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                summarySheet.Cells["A3"].Value = "Period:";
                summarySheet.Cells["B3"].Value = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";

                summarySheet.Cells["A4"].Value = "Top N Products:";
                summarySheet.Cells["B4"].Value = topN;

                int summaryRow = 6;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Products Sold";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.TotalProductsSold;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Quantity Sold";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.TotalQuantitySold;

                summaryRow++;
                summarySheet.Cells[$"A{summaryRow}"].Value = "Total Revenue";
                summarySheet.Cells[$"B{summaryRow}"].Value = report.TotalRevenue;
                summarySheet.Cells[$"B{summaryRow}"].Style.Numberformat.Format = "₱#,##0.00";

                summarySheet.Cells["A6:A8"].Style.Font.Bold = true;
                summarySheet.Cells["A1:B8"].AutoFitColumns();

                // Product Rankings Sheet
                var rankingsSheet = package.Workbook.Worksheets.Add("Product Rankings");

                // Headers
                rankingsSheet.Cells["A1"].Value = "Rank";
                rankingsSheet.Cells["B1"].Value = "Product";
                rankingsSheet.Cells["C1"].Value = "Category";
                rankingsSheet.Cells["D1"].Value = "Qty Sold";
                rankingsSheet.Cells["E1"].Value = "Revenue";
                rankingsSheet.Cells["F1"].Value = "Stock";
                rankingsSheet.Cells["G1"].Value = "Trend";

                // Style headers
                using (var range = rankingsSheet.Cells["A1:G1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Data
                int row = 2;
                foreach (var product in report.Products)
                {
                    rankingsSheet.Cells[row, 1].Value = product.Rank;
                    rankingsSheet.Cells[row, 2].Value = product.ProductName;
                    rankingsSheet.Cells[row, 3].Value = product.CategoryName;
                    rankingsSheet.Cells[row, 4].Value = product.QuantitySold;
                    rankingsSheet.Cells[row, 5].Value = product.Revenue;
                    rankingsSheet.Cells[row, 5].Style.Numberformat.Format = "₱#,##0.00";
                    rankingsSheet.Cells[row, 6].Value = product.CurrentStock;
                    rankingsSheet.Cells[row, 7].Value = product.Trend;

                    // Color code trend
                    switch (product.Trend.ToLower())
                    {
                        case "increasing":
                            rankingsSheet.Cells[row, 7].Style.Font.Color.SetColor(Color.Green);
                            break;
                        case "stable":
                            rankingsSheet.Cells[row, 7].Style.Font.Color.SetColor(Color.Blue);
                            break;
                        case "decreasing":
                            rankingsSheet.Cells[row, 7].Style.Font.Color.SetColor(Color.Red);
                            break;
                    }

                    row++;
                }

                // Add borders
                using (var range = rankingsSheet.Cells[1, 1, row - 1, 7])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                rankingsSheet.Cells[rankingsSheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
        #endregion

        #region BackUp Database
        public async Task<BackupListResponse> GetAllBackupsAsync()
        {
            try
            {
                return await _getRepo.GetAllBackupsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting backups");
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
                _logger.LogInformation($"Downloading backup: {fileName}");
                return await _getRepo.DownloadBackupAsync(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading backup");
                return null;
            }
        }

        public async Task<BackupSettingsDto> GetBackupSettingsAsync()
        {
            return new BackupSettingsDto
            {
                AutoBackupEnabled = _config.GetValue<bool>("BackupSettings:AutoBackupEnabled", true),
                AutoBackupTime = new TimeSpan(
                    _config.GetValue<int>("BackupSettings:AutoBackupHour", 2),
                    _config.GetValue<int>("BackupSettings:AutoBackupMinute", 0),
                    0
                ),
                RetentionDays = _config.GetValue<int>("BackupSettings:RetentionDays", 30),
                MaxBackupCount = _config.GetValue<int>("BackupSettings:MaxBackupCount", 50)
            };
        }

        #endregion
    }
}

