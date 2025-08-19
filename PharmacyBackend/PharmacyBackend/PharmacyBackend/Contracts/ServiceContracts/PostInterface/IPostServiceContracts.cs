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
        #region Employee
        public Task<int> CreateEmployeeAsync(CreateEmployeeRequest request);
        public Task<string> AuthenticationAsync(LogInRequest request);
        #endregion

        #region Account
        public Task<bool> CreateAccountAsync(CreateAccountRequest request);
        #endregion

        //sales
        #region Sales
        public Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto request);
        #endregion

        //Discount
        #region Discount
        public Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);
        #endregion

        //Product
        #region Product
        public Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        #endregion

        //Category
        #region Category
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        #endregion

        //ItemDataControl

        //Brand
        #region Brand
        public Task<BrandDto> CreateBrandAsync(CreateBrandDto dto);
        #endregion

        //Generic
        #region Generic
        public Task<GenericDto> CreateGenericAsync(CreateGenericDto dto);
        #endregion

        //Formulation
        #region Formulation
        public Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto);
        #endregion

        //Classification
        #region Classification
        public Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto);
        #endregion
        //Type
        #region Type
        public Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto);
        #endregion
    }
}
