using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.PostInterface
{
    public interface IPostRepositoryContracts
    {
        //employee
        public Task<bool> CreateEmployeeAsync(Employee request);
        public Task<bool> AuthenticationAsync(LogInRequest request);
        public string TokenGeneration(Account request);

        //sales
        public Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction transaction);

        //Discount
        public Task<Discount> CreateDiscountAsync(Discount discount);

        //Product
        public Task<Product> CreateProductAsync(Product product);

        //Category
        public Task<Category> CreateCategoryAsync(Category category);

        //ItemDataControl

        //Brand
        public Task<Brand> CreateBrandAsync(Brand brand);

        //Generic
        public Task<Generic> CreateGenericAsync(Generic generic);

        //Formulation
        public Task<Formulation> CreateFormulationAsync(Formulation formulation);

        //Classification
        public Task<Classification> CreateClassificationAsync(Classification classification);

        //Types
        public Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel);
    }
}

