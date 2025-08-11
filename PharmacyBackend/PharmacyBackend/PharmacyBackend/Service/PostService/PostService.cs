using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
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

        public Task<bool> AuthenticationAsync(LogInRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            throw new NotImplementedException();
        }

        public string TokenGeneration(Account request)
        {
            throw new NotImplementedException();
        }

        //sale 

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

        //Discount
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
        //Product
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

        //Category
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

        //ItemDataControl

        //Brand
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

        //Generic

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

        //Formulation
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

        //Classification
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

        //Type
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

    }
}
