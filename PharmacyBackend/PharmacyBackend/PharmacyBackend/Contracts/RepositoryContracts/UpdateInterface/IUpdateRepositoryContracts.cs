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
    }
}
