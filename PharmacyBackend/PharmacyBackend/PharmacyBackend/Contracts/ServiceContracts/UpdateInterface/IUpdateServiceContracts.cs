using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Contracts.ServiceContracts.UpdateInterface
{
    public interface IUpdateServiceContracts
    {
        //employee
        public Task<bool> UpdateEmployeeAsync(UpdateEmployeeRequest request);

        //sales
        public Task<bool> UpdateSalesTransactionAsync(int id, UpdateSalesTransactionDto request);

        //Discount
        public Task<bool> UpdateDiscountAsync(int id, UpdateDiscountDto dto);
    }
}
