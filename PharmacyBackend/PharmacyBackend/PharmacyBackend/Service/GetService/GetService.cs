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

namespace PharmacyBackend.Service.GetService
{
    public class GetService : IGetServiceContracts
    {
        private readonly IGetRepositoryContracts _getRepo;

        public GetService(IGetRepositoryContracts getRepo)
        {
            _getRepo = getRepo;
        }

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
                RolePosition = e.Role.RolePosition
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
                RolePosition = employee.Role.RolePosition
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
                SupplierID = p.SupplierID,
                Price = p.Price,
                IsActive = p.IsActive,
                DateCreated = p.DateCreated,

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
                SupplierName = p.Supplier?.SupplierName ?? string.Empty
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
                SupplierID = p.SupplierID,
                Price = p.Price,
                IsActive = p.IsActive,
                DateCreated = p.DateCreated,

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
                SupplierName = p.Supplier?.SupplierName ?? string.Empty
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
                Email = s.Email,
                Address = s.Address,
                IsActive = s.IsActive
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
                Email = s.Email,
                Address = s.Address,
                IsActive = s.IsActive
            };
        }
        #endregion

        #region Inventory 
        public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
        {
            var inventories = await _getRepo.GetAllInventoryAsync();
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
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    UnitCost = poi.UnitCost,
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
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    UnitCost = poi.UnitCost,
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
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    UnitCost = poi.UnitCost,
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
                    ProductName = poi.Product.ProductName,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    UnitCost = poi.UnitCost,
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
                    ProductName = poi.Product.ProductName,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    BatchNumber = poi.BatchNumber,
                    UnitCost = poi.UnitCost,
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

    }
}

