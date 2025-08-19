using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface
{
    public interface IUpdateRepositoryContracts
    {
        //employee
        #region Employee
        public Task<bool> UpdateEmployeeAsync(Employee request);
        public Task<bool> UpdateAccountAsync(Account request);
        #endregion

        //sales
        #region Sales
        public Task<bool> UpdateSalesTransactionAsync(SalesTransaction transaction);
        #endregion

        //Discount
        #region Discount
        public Task<bool> UpdateDiscountAsync(Discount discount);
        #endregion

        //Product
        #region Product
        public Task<bool> UpdateProductAsync(Product product);
        #endregion

        //Category
        #region Category
        public Task<bool> UpdateCategoryAsync(Category category);
        #endregion

        //ItemDataControl

        //Brand
        #region Brand
        public Task<bool> UpdateBrandAsync(Brand brand);
        #endregion

        //Generic
        #region Generic
        public Task<bool> UpdateGenericAsync(Generic generic);
        #endregion

        //Formulation
        #region Formulation
        public Task<bool> UpdateFormulationAsync(Formulation formulation);
        #endregion

        //Classification
        #region Classification
        public Task<bool> UpdateClassificationAsync(Classification classification);
        #endregion

        //TypeModel
        #region TypeModel
        public Task<bool> UpdateTypeModelAsync(TypeModel typeModel);
        #endregion
    }
}
