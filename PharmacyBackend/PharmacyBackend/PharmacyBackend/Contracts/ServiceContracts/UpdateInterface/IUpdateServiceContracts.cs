using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
﻿using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;

namespace PharmacyBackend.Contracts.ServiceContracts.UpdateInterface
{
    public interface IUpdateServiceContracts
    {
        #region Employee
        public Task<bool> UpdateEmployeeAsync(UpdateEmployeeRequest request);
        public Task<bool> UpdateAccountAsync(UpdateAccountRequest request);
        public Task<bool> ArchiveEmployeeByIdAsync(ArchiveEmployeeHistoryRequest request);
        #endregion

        #region Account
        public Task<bool> ResetAccountPasswordAsync(int accountId);
        public Task<bool> UpdateEmployeeAccountAsync(UpdateEmployeeAccountRequest request);
        #endregion

        #region Sales
        public Task<bool> UpdateSalesTransactionAsync(int id, UpdateSalesTransactionDto dto);
        #endregion

        #region Discount
        public Task<bool> UpdateDiscountAsync(int id, UpdateDiscountDto dto);
        public Task<bool> ArchiveDiscountByIdAsync(int discountId);
        #endregion

        #region Product
        public Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);
        public Task<bool> ArchiveProductByIdAsync(int productId);
        #endregion

        #region Category
        public Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        public Task<bool> ArchiveCategoryByIdAsync(int categoryId);
        #endregion

        #region Brand
        public Task<bool> UpdateBrandAsync(int id, UpdateBrandDto dto);
        public Task<bool> ArchiveBrandByIdAsync(int brandId);
        #endregion

        #region Generic
        public Task<bool> UpdateGenericAsync(int id, UpdateGenericDto dto);
        public Task<bool> ArchiveGenericByIdAsync(int genericId);
        #endregion

        #region Formulation
        public Task<bool> UpdateFormulationAsync(int id, UpdateFormulationDto dto);
        public Task<bool> ArchiveFormulationByIdAsync(int formulationId);
        #endregion

        #region Classification
        public Task<bool> UpdateClassificationAsync(int id, UpdateClassificationDto dto);
        public Task<bool> ArchiveClassificationByIdAsync(int classificationId);
        #endregion

        #region Type
        public Task<bool> UpdateTypeModelAsync(int id, UpdateTypeModelDto dto);
        public Task<bool> ArchiveTypeModelByIdAsync(int typeId);
        #endregion

        #region Supplier
        public Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto dto);
        public Task<bool> ArchiveSupplierByIdAsync(int supplierId);
        public Task<bool> AddProductsToSupplierAsync(AssignSupplierProductRequest request);
        #endregion

        #region Inventory
        public Task<bool> UpdateInventoryAsync(int id, UpdateInventoryDto dto);
        public Task<bool> ArchiveInventoryByIdAsync(int inventoryId);
        #endregion

        #region VAT
        public Task<bool> UpdateVATAsync(int id, UpdateVATDto dto);
        public Task<bool> ArchiveVATByIdAsync(int vatId);
        #endregion

        #region PurchaseOrder
        public Task<bool> UpdatePurchaseOrderAsync(int id, UpdatePurchaseOrderDto dto);
        public Task<bool> MarkAsReceivedAsync(PurchaseOrderReceivedRequest request);
        public Task<bool> ArchivePurchaseOrderByIdAsync(int id);
        #endregion

        #region BackUp Database
        Task<bool> UpdateBackupSettingsAsync(BackupSettingsDto settings);
        #endregion
    }
}
