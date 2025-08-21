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
                AccountCode = a.AccountCode,
                Username = a.Username,
                DecryptedPassword = a.EncryptedPassword,
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
                AccountCode = account.AccountCode,
                Username = account.Username,
                DecryptedPassword = account.EncryptedPassword,
            };
            return modifAccount;
        }


        #endregion

        #region Sales
        public async Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync()
        {
            var transactions = await _getRepo.GetAllSalesTransactionsAsync();
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                NetAmount = t.NetAmount,
                DiscountId = t.DiscountId,
                DiscountName = t.Discount?.DiscountName ?? string.Empty,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            }).ToList();
        }

        public async Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id)
        {
            var transaction = await _getRepo.GetSalesTransactionByIdAsync(id);
            if (transaction is null)
                return null;
            return new SalesTransactionDto
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                InvoiceNumber = transaction.InvoiceNumber,
                TotalAmount = transaction.TotalAmount,
                DiscountAmount = transaction.DiscountAmount,
                NetAmount = transaction.NetAmount,
                DiscountId = transaction.DiscountId,
                DiscountName = transaction.Discount?.DiscountName ?? string.Empty,
                Items = transaction.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            };
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
                DiscountRate = d.DiscountRate
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
                DiscountRate = d.DiscountRate
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

                // CHANGED from InventoryItem to Inventory
                TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.QuantityOnHand) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
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

        public async Task<ProductDto> GetProductByIdAsync(int id)
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

                // CHANGED from InventoryItem to Inventory
                TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.QuantityOnHand) ?? 0,
                IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.QuantityOnHand) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
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
        public async Task<IEnumerable<SupplierDto>> GetAllSupplierAsync()
        {
            var suppliers = await _getRepo.GetAllSupplierAsync();
            return suppliers.Select(s => new SupplierDto
            {
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                ContactNumber = s.ContactNumber,
                Email = s.Email,
                Address = s.Address,
                IsActive = s.IsActive
            });
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
        {
            var s = await _getRepo.GetSupplierByIdAsync(id);
            if (s == null)
                return null;
            return new SupplierDto
            {
                SupplierID = s.SupplierID,
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
                QuantityOnHand = i.QuantityOnHand,
                ReorderLevel = i.ReorderLevel,
                CostPrice = i.CostPrice,
                SellingPrice = i.SellingPrice,
                DateAdded = i.DateAdded,
                IsActive = i.IsActive,
                IsExpired = i.IsExpired,
                IsNearExpiry = i.IsNearExpiry,
                IsCriticalStock = i.IsCriticalStock
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
        public async Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrderAsync()
        {
            var orders = await _getRepo.GetAllPurchaseOrderAsync();
            return orders.Select(po => new PurchaseOrderDto
            {
                PurchaseOrderID = po.PurchaseOrderID,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierID = po.SupplierID,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemID = poi.PurchaseOrderItemID,
                    ProductID = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
        }

        public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id)
        {
            var po = await _getRepo.GetPurchaseOrderByIdAsync(id);
            if (po == null)
                return null;
            return new PurchaseOrderDto
            {
                PurchaseOrderID = po.PurchaseOrderID,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierID = po.SupplierID,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemID = poi.PurchaseOrderItemID,
                    ProductID = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.TotalCost
                }).ToList()
            };
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderBySupplierIdAsync(int supplierId)
        {
            var orders = await _getRepo.GetPurchaseOrderBySupplierIdAsync(supplierId);
            return orders.Select(po => new PurchaseOrderDto
            {
                PurchaseOrderID = po.PurchaseOrderID,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierID = po.SupplierID,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemID = poi.PurchaseOrderItemID,
                    ProductID = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrderByStatusAsync(string status)
        {
            var orders = await _getRepo.GetPurchaseOrderByStatusAsync(status);
            return orders.Select(po => new PurchaseOrderDto
            {
                PurchaseOrderID = po.PurchaseOrderID,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                OrderDate = po.OrderDate,
                DeliveryDate = po.DeliveryDate,
                SupplierID = po.SupplierID,
                SupplierName = po.Supplier?.SupplierName ?? string.Empty,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemID = poi.PurchaseOrderItemID,
                    ProductID = poi.ProductID,
                    ProductName = poi.Product?.ProductName ?? string.Empty,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.TotalCost
                }).ToList()
            });
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
                QuantityChanged = sm.QuantityChanged,
                QuantityBefore = sm.QuantityBefore,
                QuantityAfter = sm.QuantityAfter,
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
                QuantityChanged = sm.QuantityChanged,
                QuantityBefore = sm.QuantityBefore,
                QuantityAfter = sm.QuantityAfter,
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
                QuantityChanged = sm.QuantityChanged,
                QuantityBefore = sm.QuantityBefore,
                QuantityAfter = sm.QuantityAfter,
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
                QuantityChanged = sm.QuantityChanged,
                QuantityBefore = sm.QuantityBefore,
                QuantityAfter = sm.QuantityAfter,
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

