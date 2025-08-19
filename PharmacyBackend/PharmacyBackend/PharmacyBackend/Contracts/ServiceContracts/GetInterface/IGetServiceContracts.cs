using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
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
        #region Employee
        public Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        public Task<List<AccountDTO>> GetAllAccountsAsync();
        public Task<AccountDTO> GetAccountByIdAsync(int id);
        public Task<bool> AccountUsernameValidation(string userName);
        public Task<List<EmployeeDTO>> GetEmployeeByLastName(string lastName);
        #endregion

        //sales
        #region Sales
        public Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync();
        public Task<SalesTransactionDto> GetSalesTransactionByIdAsync(int id);
        #endregion

        //DISCOUNT
        #region Discount
        public Task<IEnumerable<DiscountDto>> GetAllDiscountAsync();
        public Task<DiscountDto> GetDiscountByIdAsync(int id);
        #endregion

        //PRODUCT
        #region Product
        public Task<IEnumerable<ProductDto>> GetAllProductAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        #endregion

        //Category
        #region Category
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);
        #endregion

        //ItemDataControl
        //Brand
        #region Brand
        public Task<IEnumerable<BrandDto>> GetAllBrandAsync();
        public Task<BrandDto> GetBrandByIdAsync(int id);
        #endregion

        //Generic
        #region Generic
        public Task<IEnumerable<GenericDto>> GetAllGenericAsync();
        public Task<GenericDto> GetGenericByIdAsync(int id);
        #endregion

        //Formulation
        #region Formulation
        public Task<IEnumerable<FormulationDto>> GetAllFormulationAsync();
        public Task<FormulationDto> GetFormulationByIdAsync(int id);
        #endregion

        //Classificaton
        #region Classification
        public Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync();
        public Task<ClassificationDto> GetClassificationByIdAsync(int id);
        #endregion

        //Type
        #region Type
        public Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync();
        public Task<TypeModelDto> GetTypeModelByIdAsync(int id);
        #endregion
    }
}
