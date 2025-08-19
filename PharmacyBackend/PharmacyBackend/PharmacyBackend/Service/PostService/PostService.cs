using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models.CashierViewModel;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;

namespace PharmacyBackend.Service.PostService
{
    public class PostService : IPostServiceContracts
    {
        private readonly IPostRepositoryContracts _postRepo;

        public PostService(IPostRepositoryContracts postRepo)
        {
            _postRepo = postRepo;
        }

        #region Account
        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var enteredPassword = AesHelper.Encrypt(request.Password);
            request.Password = enteredPassword;
            var token = await _postRepo.AuthenticationAsync(request);
            return token;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountRequest request)
        {
            if (request is null)
                return false;

            var encryptedPass = AesHelper.Encrypt(request.Password);
            var newAcc = new Account()
            {
                AccountCode = request.AccountCode,
                Username = request.Username,
                EncryptedPassword = encryptedPass,
                EmployeeId = request.EmployeeId
            };

            var createAccount = await _postRepo.CreateAccountAsync(newAcc);
            if (!createAccount)
                return false;
            return createAccount;
        }
        #endregion

        #region Employee
        public async Task<int> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (request is null)
                return 0;

            int roleId = 0;
            switch (request.RolePosition)
            {
                case "Administrator":
                    roleId = 1;
                    break;
                case "Cashier":
                    roleId = 2;
                    break;
                case "Inventory Manager":
                    roleId = 3;
                    break;
            }

            var newEmployee = new Employee
            {
                EmployeeCode = request.EmployeeCode,
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                Age = request.Age,
                BirthDate = request.BirthDate,
                CivilStatus = request.CivilStatus,
                Gender = request.Gender,
                Email = request.Email,
                Address = request.Address,
                RoleId = roleId
            };

            var employeeId = await _postRepo.CreateEmployeeAsync(newEmployee);
            if (employeeId == 0)
                return 0;
            return employeeId;
        }
        #endregion

        #region Sales
        public async Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return null;

            var transaction = new SalesTransaction
            {
                TransactionDate = DateTime.Now,
                InvoiceNumber = dto.InvoiceNumber,
                DiscountId = dto.DiscountId,
                DiscountAmount = 0,
                Items = dto.Items.Select(i => new SalesItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };

            transaction.TotalAmount = transaction.Items.Sum(i => i.Subtotal);
            transaction.NetAmount = transaction.TotalAmount - transaction.DiscountAmount;

            var created = await _postRepo.CreateSalesTransactionAsync(transaction);

            return new SalesTransactionDto
            {
                Id = created.Id,
                InvoiceNumber = created.InvoiceNumber,
                TransactionDate = created.TransactionDate,
                TotalAmount = created.TotalAmount,
                NetAmount = created.NetAmount
            };
        }
        #endregion

        #region Discount
        public async Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto)
        {
            var discount = new Discount
            {
                DiscountName = dto.DiscountName,
                DiscountRate = dto.DiscountRate
            };

            var result = await _postRepo.CreateDiscountAsync(discount);
            return new DiscountDto
            {
                Id = result.Id,
                DiscountName = result.DiscountName,
                DiscountRate = result.DiscountRate
            };
        }
        #endregion

        #region Product
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryID = dto.CategoryID,
                BrandID = dto.BrandID,
                GenericID = dto.GenericID,
                FormulationID = dto.FormulationID,
                ClassificationID = dto.ClassificationID,
                TypeID = dto.TypeID,
                SupplierID = dto.SupplierID,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = true
            };

            var created = await _postRepo.CreateProductAsync(product);

            return new ProductDto
            {
                ProductID = created.ProductID,
                ProductName = created.ProductName,
                CategoryID = created.CategoryID,
                BrandID = created.BrandID,
                GenericID = created.GenericID,
                FormulationID = created.FormulationID,
                ClassificationID = created.ClassificationID,
                TypeID = created.TypeID,
                SupplierID = created.SupplierID,
                Price = created.Price,
                Stock = created.Stock,
                IsActive = created.IsActive
            };
        }
        #endregion

        #region Category
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName
            };

            var created = await _postRepo.CreateCategoryAsync(category);

            return new CategoryDto
            {
                CategoryID = created.CategoryID,
                CategoryName = created.CategoryName
            };
        }
        #endregion

        #region Brand
        public async Task<BrandDto> CreateBrandAsync(CreateBrandDto dto)
        {
            var brand = new Brand
            {
                BrandName = dto.BrandName
            };
            var created = await _postRepo.CreateBrandAsync(brand);

            return new BrandDto
            {
                BrandID = created.BrandID,
                BrandName = created.BrandName
            };
        }
        #endregion

        #region Generic
        public async Task<GenericDto> CreateGenericAsync(CreateGenericDto dto)
        {
            var generic = new Generic
            {
                GenericName = dto.GenericName
            };
            var created = await _postRepo.CreateGenericAsync(generic);

            return new GenericDto
            {
                GenericID = created.GenericID,
                GenericName = created.GenericName
            };
        }
        #endregion

        #region Formulation
        public async Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto)
        {
            var formulation = new Formulation
            {
                FormulationName = dto.FormulationName
            };
            var created = await _postRepo.CreateFormulationAsync(formulation);

            return new FormulationDto
            {
                FormulationID = created.FormulationID,
                FormulationName = created.FormulationName
            };
        }
        #endregion

        #region Classification
        public async Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto)
        {
            var classification = new Classification
            {
                ClassificationName = dto.ClassificationName
            };
            var created = await _postRepo.CreateClassificationAsync(classification);

            return new ClassificationDto
            {
                ClassificationID = created.ClassificationID,
                ClassificationName = created.ClassificationName
            };
        }
        #endregion

        #region Type
        public async Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto)
        {
            var type = new TypeModel
            {
                TypeName = dto.TypeName
            };
            var created = await _postRepo.CreateTypeModelAsync(type);

            return new TypeModelDto
            {
                TypeID = created.TypeID,
                TypeName = created.TypeName
            };
        }
        #endregion
    }
}
