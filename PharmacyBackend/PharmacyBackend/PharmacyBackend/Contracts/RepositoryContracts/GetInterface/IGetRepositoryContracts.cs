using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.GetInterface
{
    public interface IGetRepositoryContracts
    {
        //employee
        public Task<List<Employee>> GetAllEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);

        //sales
        public Task<List<SalesTransaction>> GetAllSalesTransactionsAsync();
        public Task<SalesTransaction?> GetSalesTransactionByIdAsync(int id);

        //Discount
        public Task<IEnumerable<Discount>> GetAllDiscountAsync();
        public Task<Discount> GetDiscountByIdAsync(int id);

        //Product
        public Task<IEnumerable<Product>> GetAllProductAsync();
        public Task<Product> GetProductByIdAsync(int id);

        //Category
        public Task<IEnumerable<Category>> GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id);

        //Item Data Control

        //Brand
        public Task<IEnumerable<Brand>> GetAllBrandAsync();
        public Task<Brand> GetBrandByIdAsync(int id);

        //Generic
        public Task<IEnumerable<Generic>> GetAllGenericAsync();
        public Task<Generic> GetGenericByIdAsync(int id);

        //Formulation
        public Task<IEnumerable<Formulation>> GetAllFormulationAsync();
        public Task<Formulation> GetFormulationByIdAsync(int id);

        //Classification
        public Task<IEnumerable<Classification>> GetAllClassificationAsync();
        public Task<Classification> GetClassificationByIdAsync(int id);

        //Types
        public Task<IEnumerable<TypeModel>> GetAllTypeModelAsync();
        public Task<TypeModel> GetTypeModelByIdAsync(int id);
    }
}
