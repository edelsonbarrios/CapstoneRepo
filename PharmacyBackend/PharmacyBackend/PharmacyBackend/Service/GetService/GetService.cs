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
                Address = employee.Address
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
                Stock = p.Stock,
                IsActive = p.IsActive
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
                Stock = p.Stock,
                IsActive = p.IsActive
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
    }
}

