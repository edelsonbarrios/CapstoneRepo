using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using System.Threading.Tasks;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
        //employee
        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<bool> AuthenticationAsync(LogInRequest request);
        public string TokenGeneration(Account request);

        //sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);

        //Discount
        public Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);

        //Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);

        //Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);

        //ItemDataControl

        //Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);

        //Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);

        //Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);

        //Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        //Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);
    }
}
