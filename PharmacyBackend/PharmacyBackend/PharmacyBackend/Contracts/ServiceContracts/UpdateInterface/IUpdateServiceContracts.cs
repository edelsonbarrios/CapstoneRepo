using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
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

        //Product
        public Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);

        //Category
        public Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto);

        //ItemDataControl

        //Brand
        public Task<bool> UpdateBrandAsync(int id, UpdateBrandDto dto);

        //Generic
        public Task<bool> UpdateGenericAsync(int id, UpdateGenericDto dto);

        //Formulation
        public Task<bool> UpdateFormulationAsync(int id, UpdateFormulationDto dto);

        //Classification
        public Task<bool> UpdateClassificationAsync(int id, UpdateClassificationDto dto);

        //Type
        public Task<bool> UpdateTypeModelAsync(int id, UpdateTypeModelDto dto);
    }
}
