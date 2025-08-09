using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using System.Threading.Tasks;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.PostInterface
{
    public interface IPostServiceContracts
    {
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
    }
}
