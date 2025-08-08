using Microsoft.AspNetCore.Mvc.ModelBinding;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Service.UpdateService
{
    public class UpdateService : IUpdateServiceContracts
    {
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly IGetRepositoryContracts _getRepo;

        public UpdateService(IUpdateRepositoryContracts updateRepo, 
            IGetRepositoryContracts getRepo)
        {
            _updateRepo = updateRepo;
            _getRepo = getRepo;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            if (request is null)
                return false;

            int roleId = 0;

            switch(request.RolePosition)
            {
                case "Administrator":
                    roleId = 1;
                    break;
                case "Cashier":
                    roleId = 2;
                    break;
                case "Inventory Manager":
                    roleId = 3;
                    break;
            }

            var employeeToUpdate = await _getRepo.GetEmployeeByIdAsync(request.Id);
            if (employeeToUpdate is null)
                return false;


            employeeToUpdate.EmployeeCode = request.EmployeeCode;
            employeeToUpdate.LastName = request.LastName;
            employeeToUpdate.FirstName = request.FirstName;
            employeeToUpdate.MiddleName = request.MiddleName;
            employeeToUpdate.Age = request.Age;
            employeeToUpdate.BirthDate = request.BirthDate;
            employeeToUpdate.CivilStatus = request.CivilStatus;
            employeeToUpdate.Gender = request.Gender;
            employeeToUpdate.Email = request.Email;
            employeeToUpdate.Address = request.Address;
            employeeToUpdate.RoleId = roleId;

            var updateEmployee = await _updateRepo.UpdateEmployeeAsync(employeeToUpdate);
            if (!updateEmployee)
                return false;
            return true;
        }

        //sales 
        public async Task<bool> UpdateSalesTransactionAsync(int id, UpdateSalesTransactionDto dto)
        {
            var existing = await _getRepo.GetSalesTransactionByIdAsync(id);
            if (existing == null) return false;

            existing.DiscountAmount = dto.DiscountAmount;

            foreach (var updatedItem in dto.Items)
            {
                var item = existing.Items.FirstOrDefault(i => i.Id == updatedItem.Id);
                if (item != null)
                {
                    item.Quantity = updatedItem.Quantity;
                    item.UnitPrice = updatedItem.UnitPrice;
                    item.Subtotal = updatedItem.Quantity * updatedItem.UnitPrice;
                }
            }

            existing.TotalAmount = existing.Items.Sum(i => i.Subtotal);
            existing.NetAmount = existing.TotalAmount - existing.DiscountAmount;

            return await _updateRepo.UpdateSalesTransactionAsync(existing);
        }

        //Discount
        public async Task<bool> UpdateDiscountAsync(int id, UpdateDiscountDto dto)
        {
            var existing = await _getRepo.GetDiscountByIdAsync(id);
            if (existing == null) return false;

            existing.DiscountName = dto.DiscountName;
            existing.DiscountRate = dto.DiscountRate;

            await _updateRepo.UpdateDiscountAsync(existing);
            return true;
        }

        //Product
        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var existing = await _getRepo.GetProductByIdAsync(id);
            if (existing == null) return false;

            existing.ProductName = dto.ProductName;
            existing.CategoryID = dto.CategoryID;
            existing.BrandID = dto.BrandID;
            existing.GenericID = dto.GenericID;
            existing.FormulationID = dto.FormulationID;
            existing.ClassificationID = dto.ClassificationID;
            existing.TypeID = dto.TypeID;
            existing.SupplierID = dto.SupplierID;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateProductAsync(existing);
            return true;
        }
    }
}
