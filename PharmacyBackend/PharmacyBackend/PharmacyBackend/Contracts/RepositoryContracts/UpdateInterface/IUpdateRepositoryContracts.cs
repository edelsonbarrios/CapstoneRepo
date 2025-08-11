using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface
{
    public interface IUpdateRepositoryContracts
    {
        //employee
        public Task<bool> UpdateEmployeeAsync(Employee request);

        //sales
        public Task<bool> UpdateSalesTransactionAsync(SalesTransaction transaction);

        //Discount
        public Task<bool> UpdateDiscountAsync(Discount discount);

        //Product
        public Task<bool> UpdateProductAsync(Product product);

        //Category
        public Task<bool> UpdateCategoryAsync(Category category);

        //ItemDataControl

        //Brand
        public Task<bool> UpdateBrandAsync(Brand brand);

        //Generic
        public Task<bool> UpdateGenericAsync(Generic generic);

        //Formulation
        public Task<bool> UpdateFormulationAsync(Formulation formulation);

        //Classification
        public Task<bool> UpdateClassificationAsync(Classification classification);

        //TypeModel
        public Task<bool> UpdateTypeModelAsync(TypeModel typeModel);
    }
}
