using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using System.Collections;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
namespace PharmacyBackend.Contracts.ServiceContracts.GetInterface
{
    public interface IGetServiceContracts
    {
        //employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);

        //sales
        public Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id);

        //DISCOUNT
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);

        //PRODUCT
        public Task<IEnumerable<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);

        //Category
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);

        //ItemDataControl
        //Brand
        public Task<IEnumerable<BrandDto>> GetAllBrandAsync();
        public Task<BrandDto> GetBrandByIdAsync(int id);

        //Generic
        public Task<IEnumerable<GenericDto>> GetAllGenericAsync();
        public Task<GenericDto> GetGenericByIdAsync(int id);

        //Formulation
        public Task<IEnumerable<FormulationDto>> GetAllFormulationAsync();
        public Task<FormulationDto> GetFormulationByIdAsync(int id);

        //Classificaton
        public Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync();
        public Task<ClassificationDto> GetClassificationByIdAsync(int id);

        //Type
        public Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync();
        public Task<TypeModelDto> GetTypeModelByIdAsync(int id);
    }
}
