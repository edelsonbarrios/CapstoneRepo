using Microsoft.AspNetCore.Mvc.ModelBinding;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using System.Security.Principal;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using System.Runtime.InteropServices;

namespace PharmacyBackend.Service.UpdateService
{
    public class UpdateService : IUpdateServiceContracts
    {
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IPostRepositoryContracts _postRepo;

        public UpdateService(IUpdateRepositoryContracts updateRepo,
            IGetRepositoryContracts getRepo,
            IPostRepositoryContracts postRepo)
        {
            _updateRepo = updateRepo;
            _getRepo = getRepo;
            _postRepo = postRepo;
        }
        #region Account
        public async Task<bool> UpdateAccountAsync(UpdateAccountRequest request)
        {
            if (request is null)
                return false;

            var account = await _getRepo.GetAccountByIdAsync(request.Id);
            if (account is null)
                return false;

            var encryptedPassword = AesHelper.Encrypt(request.EncryptedPassword);

            account.Username = request.Username;
            account.EncryptedPassword = encryptedPassword;

            var updateAccount = await _updateRepo.UpdateAccountAsync(account);
            if (!updateAccount)
                return false;
            return true;
        }
        #endregion

        #region Employee
        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            if (request is null)
                return false;

            int roleId = 0;

            switch (request.RolePosition)
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
        public async Task<bool> ArchiveEmployeeByIdAsync(ArchiveEmployeeHistoryRequest request)
        {
            var employee = await _getRepo.GetEmployeeByIdAsync(request.EmployeeId);
            if (employee is null)
                return false;
            employee.IsActive = false;

            var updateEmployee = await _updateRepo.UpdateEmployeeAsync(employee);
            if (!updateEmployee)
                return false;

            var newEmployeeHistory = new EmployeeHistory()
            {
                EmployeeId = request.EmployeeId,
                ActionPerformed = "Archived",
                PerformedBy = request.PerformedBy,
                PerformedAt = DateTime.UtcNow
            };
            var createEmployeeHistory = await _postRepo.CreateEmployeeHistoryAsync(newEmployeeHistory);
            if (!createEmployeeHistory)
                return false;

            return true;
        }
        #endregion

        #region Sales
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
        #endregion

        #region Discount
        public async Task<bool> UpdateDiscountAsync(int id, UpdateDiscountDto dto)
        {
            var existing = await _getRepo.GetDiscountByIdAsync(id);
            if (existing == null) return false;

            existing.DiscountName = dto.DiscountName;
            existing.DiscountRate = dto.DiscountRate;

            await _updateRepo.UpdateDiscountAsync(existing);
            return true;
        }
        public async Task<bool> ArchiveDiscountByIdAsync(int discountId)
        {
            var discount = await _getRepo.GetDiscountByIdAsync(discountId);
            if (discount is null)
                return false;

            discount.IsActive = false;

            var updateDiscount = await _updateRepo.UpdateDiscountAsync(discount);
            if (!updateDiscount)
                return false;

            return updateDiscount;
        }
        #endregion

        #region Product
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
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateProductAsync(existing);
            return true;
        }
        #endregion

        #region Category
        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var existing = await _getRepo.GetCategoryByIdAsync(id);
            if (existing == null) return false;

            existing.CategoryName = dto.CategoryName;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateCategoryAsync(existing);
            return true;
        }
        #endregion

        #region Brand
        public async Task<bool> UpdateBrandAsync(int id, UpdateBrandDto dto)
        {
            var existing = await _getRepo.GetBrandByIdAsync(id);
            if (existing == null) return false;

            existing.BrandName = dto.BrandName;

            await _updateRepo.UpdateBrandAsync(existing);
            return true;
        }
        #endregion

        #region Generic
        public async Task<bool> UpdateGenericAsync(int id, UpdateGenericDto dto)
        {
            var existing = await _getRepo.GetGenericByIdAsync(id);
            if (existing == null) return false;

            existing.GenericName = dto.GenericName;

            await _updateRepo.UpdateGenericAsync(existing);
            return true;
        }
        #endregion

        #region Formulation
        public async Task<bool> UpdateFormulationAsync(int id, UpdateFormulationDto dto)
        {
            var existing = await _getRepo.GetFormulationByIdAsync(id);
            if (existing == null) return false;

            existing.FormulationName = dto.FormulationName;

            await _updateRepo.UpdateFormulationAsync(existing);
            return true;
        }
        #endregion

        #region Classification
        public async Task<bool> UpdateClassificationAsync(int id, UpdateClassificationDto dto)
        {
            var existing = await _getRepo.GetClassificationByIdAsync(id);
            if (existing == null) return false;

            existing.ClassificationName = dto.ClassificationName;

            await _updateRepo.UpdateClassificationAsync(existing);
            return true;
        }
        #endregion

        #region Type
        public async Task<bool> UpdateTypeModelAsync(int id, UpdateTypeModelDto dto)
        {
            var existing = await _getRepo.GetTypeModelByIdAsync(id);
            if (existing == null) return false;

            existing.TypeName = dto.TypeName;

            await _updateRepo.UpdateTypeModelAsync(existing);
            return true;
        }
        #endregion

        #region Supplier
        public async Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto dto)
        {
            var existing = await _getRepo.GetSupplierByIdAsync(id);
            if (existing == null) return false;

            existing.SupplierName = dto.SupplierName;
            existing.ContactNumber = dto.ContactNumber;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateSupplierAsync(existing);
            return true;
        }
        #endregion

        #region Inventory
        public async Task<bool> UpdateInventoryAsync(int id, UpdateInventoryDto dto)
        {
            var existing = await _getRepo.GetInventoryByIdAsync(id);
            if (existing == null) return false;

            existing.ProductID = dto.ProductID;
            existing.BatchNumber = dto.BatchNumber;
            existing.ManufacturingDate = dto.ManufacturingDate;
            existing.ExpirationDate = dto.ExpirationDate;
            existing.QuantityOnHand = dto.QuantityOnHand;
            existing.ReorderLevel = dto.ReorderLevel;
            existing.CostPrice = dto.CostPrice;
            existing.SellingPrice = dto.SellingPrice;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateInventoryAsync(existing);
            return true;
        }
        #endregion

        #region VAT
        public async Task<bool> UpdateVATAsync(int id, UpdateVATDto dto)
        {
            var existing = await _getRepo.GetVATByIdAsync(id);
            if (existing == null) return false;

            existing.VATName = dto.VATName;
            existing.VATRate = dto.VATRate;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateVATAsync(existing);
            return true;
        }
        public async Task<bool> ArchiveVATByIdAsync(int vatId)
        {
            var vat = await _getRepo.GetVATByIdAsync(vatId);
            if (vat is null)
                return false;

            vat.IsActive = false;

            var updateVat = await _updateRepo.UpdateVATAsync(vat);
            if (!updateVat)
                return false;

            return updateVat;
        }
        #endregion

        #region PurchaseOrder
        public async Task<bool> UpdatePurchaseOrderAsync(int id, UpdatePurchaseOrderDto dto)
        {
            var existing = await _getRepo.GetPurchaseOrderByIdAsync(id);
            if (existing == null) return false;

            existing.DeliveryDate = dto.DeliveryDate;
            existing.Status = dto.Status;
            existing.Notes = dto.Notes;

            // Update purchase order items
            foreach (var updatedItem in dto.PurchaseOrderItems)
            {
                var item = existing.PurchaseOrderItems.FirstOrDefault(poi => poi.PurchaseOrderItemID == updatedItem.PurchaseOrderItemID);
                if (item != null)
                {
                    item.QuantityReceived = updatedItem.QuantityReceived;
                    item.UnitCost = updatedItem.UnitCost;
                    item.TotalCost = item.QuantityOrdered * updatedItem.UnitCost;
                }
            }

            // Recalculate total amount
            existing.TotalAmount = existing.PurchaseOrderItems.Sum(poi => poi.TotalCost);

            await _updateRepo.UpdatePurchaseOrderAsync(existing);
            return true;

        }
        #endregion
    }
}
