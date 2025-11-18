using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using Microsoft.EntityFrameworkCore;
using PharmacyBackend.DataContext;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.Json;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;

namespace PharmacyBackend.Service.UpdateService
{
    public class UpdateService : IUpdateServiceContracts
    {
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IPostRepositoryContracts _postRepo;
        private readonly PharmacyContext _context;
        private readonly AccountHelper _accountHelper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IPostServiceContracts _postService;

        public UpdateService(IUpdateRepositoryContracts updateRepo,
            IGetRepositoryContracts getRepo,
            IPostRepositoryContracts postRepo,
            PharmacyContext context,
            AccountHelper accountHelper,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IPostServiceContracts postService
            )
        {
            _updateRepo = updateRepo;
            _getRepo = getRepo;
            _postRepo = postRepo;
            _context = context;
            _accountHelper = accountHelper;
            _configuration = configuration;
            _env = env;
            _postService = postService;
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
        public async Task<bool> ResetAccountPasswordAsync(int accountId)
        {
            if (accountId == 0) return false;

            var account = await _getRepo.GetAccountByIdAsync(accountId);
            if (account is null)
                return false;
            var rolePosition = account.Employee.Role.RolePosition;
            string password;
            switch(rolePosition)
            {
                case "Administrator":
                    password = AesHelper.Encrypt("admin");
                    account.EncryptedPassword = password;
                    break;
                case "Inventory Manager":
                    password = AesHelper.Encrypt("inventory");
                    account.EncryptedPassword = password;
                    break;
                case "Cashier":
                    password = AesHelper.Encrypt("cashier");
                    account.EncryptedPassword = password;
                    break;
            }
            var resetPassowrd = await _updateRepo.UpdateAccountAsync(account);
            if (!resetPassowrd)
                return false;
            return resetPassowrd;
        }
        public async Task<bool> UpdateEmployeeAccountAsync(UpdateEmployeeAccountRequest request)
        {
            var account = await _getRepo.GetAccountByIdAsync(request.AccountId);
            if (account is null)
                return false;

            var encryptedCurrentPassword = AesHelper.Encrypt(request.CurrentPassword);
            if (!account.EncryptedPassword.Equals(encryptedCurrentPassword))
                return false;

            if (!request.NewPassword.Equals(request.ConfirmNewPassword))
                return false;

            account.Username = request.NewUserName;
            account.EncryptedPassword = AesHelper.Encrypt(request.NewPassword);
            await _updateRepo.UpdateAccountAsync(account);
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
                    roleId = 3;
                    break;
                case "Inventory Manager":
                    roleId = 2;
                    break;
            }

            var employeeToUpdate = await _getRepo.GetEmployeeByIdAsync(request.Id);
            if (employeeToUpdate is null)
                return false;


            employeeToUpdate.EmployeeCode = request.EmployeeCode;
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
            employee.Account.IsActive = false;

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
            try
            {
                var existing = await _getRepo.GetSalesTransactionByIdAsync(id);
                if (existing == null)
                    return false;

                // Update properties
                if (dto.DiscountAmount.HasValue)
                    existing.DiscountAmount = dto.DiscountAmount.Value;

                if (!string.IsNullOrEmpty(dto.PaymentMethod))
                    existing.PaymentMethod = dto.PaymentMethod;

                if (dto.AmountPaid.HasValue)
                    existing.AmountPaid = dto.AmountPaid.Value;

                if (dto.ChangeAmount.HasValue)
                    existing.ChangeAmount = dto.ChangeAmount.Value;

                // Update items if provided
                if (dto.Items != null && dto.Items.Any())
                {
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
                }

                // Recalculate totals
                existing.TotalAmount = existing.Items.Sum(i => i.Subtotal);
                existing.NetAmount = existing.TotalAmount - existing.DiscountAmount + existing.VATAmount;

                // Update via repository
                var updated = await _updateRepo.UpdateSalesTransactionAsync(existing);
                return updated != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Discount
        public async Task<bool> UpdateDiscountAsync(int id, UpdateDiscountDto dto)
        {
            var validation = await ValidateDiscountAsync(dto.DiscountName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetDiscountByIdAsync(id);
            if (existing == null) return false;

            existing.DiscountName = dto.DiscountName;
            existing.DiscountType = dto.DiscountType;
            existing.DiscountAmount = dto.DiscountAmount;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateDiscountAsync(existing);
            return true;
        }

        public async Task<bool> ValidateDiscountAsync(string name, int discountId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateDiscountNameAsync(name, discountId);
            return validation;
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

        #region Product Helper
        private async Task<int?> GetCategoryIdByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;

            var category = await _context.Category
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
            return category?.CategoryID;
        }
        private async Task<bool> ProductExistsWithoutBrandAsync(string productName, int? excludeProductId = null)
        {
            var query = _context.Product
                .Where(p => p.ProductName == productName && p.BrandID == null);

            if (excludeProductId.HasValue)
                query = query.Where(p => p.ProductID != excludeProductId.Value);

            return await query.AnyAsync();
        }

        private async Task<bool> ProductExistsWithBrandAsync(string productName, int brandId, int? excludeProductId = null)
        {
            var query = _context.Product
                .Where(p => p.ProductName.ToLower() == productName.ToLower() && p.BrandID == brandId);

            if (excludeProductId.HasValue)
                query = query.Where(p => p.ProductID != excludeProductId.Value);

            return await query.AnyAsync();
        }

        private async Task<int?> GetBrandIdByNameAsync(string brandName)
        {
            if (string.IsNullOrEmpty(brandName))
                return null;

            var brand = await _context.Brand
                .FirstOrDefaultAsync(b => b.BrandName.ToLower() == brandName.ToLower());
            return brand?.BrandID;
        }

        private async Task<int?> GetGenericIdByNameAsync(string genericName)
        {
            if (string.IsNullOrEmpty(genericName))
                return null;

            var generic = await _context.Generic
                .FirstOrDefaultAsync(g => g.GenericName.ToLower() == genericName.ToLower());
            return generic?.GenericID;
        }

        private async Task<int?> GetFormulationIdByNameAsync(string formulationName)
        {
            if (string.IsNullOrEmpty(formulationName))
                return null;

            var formulation = await _context.Formulation
                .FirstOrDefaultAsync(f => f.FormulationName.ToLower() == formulationName.ToLower());
            return formulation?.FormulationID;
        }

        private async Task<int?> GetClassificationIdByNameAsync(string classificationName)
        {
            if (string.IsNullOrEmpty(classificationName))
                return null;

            var classification = await _context.Classification
                .FirstOrDefaultAsync(c => c.ClassificationName.ToLower() == classificationName.ToLower());
            return classification?.ClassificationID;
        }

        private async Task<int?> GetTypeIdByNameAsync(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            var type = await _context.TypeModel
                .FirstOrDefaultAsync(t => t.TypeName.ToLower() == typeName.ToLower());
            return type?.TypeID;
        }

        private async Task<int?> GetSupplierIdByNameAsync(string supplierName)
        {
            if (string.IsNullOrEmpty(supplierName))
                return null;

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(s => s.SupplierName.ToLower() == supplierName.ToLower());
            return supplier?.Id;
        }
        #endregion

        #region Product
        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var existing = await _getRepo.GetProductByIdAsync(id);
            if (existing == null) return false;

            // Resolve category first to determine if it's medicine
            var categoryId = await GetCategoryIdByNameAsync(dto.CategoryName);
            if (categoryId == null)
                throw new ArgumentException($"Category '{dto.CategoryName}' not found.");

            // Get category to check if it's medicine
            var category = await _context.Category.FindAsync(categoryId.Value);
            if (category == null)
                throw new ArgumentException($"Category not found in database.");

            bool isMedicine = category.CategoryName.Equals("Medicine", StringComparison.OrdinalIgnoreCase);

            // Medicine-specific fields
            int? brandId = null;
            int? genericId = null;
            int? formulationId = null;
            int? classificationId = null;
            int? typeId = null;

            if (isMedicine)
            {
                // For medicine, all fields are required
                brandId = await GetBrandIdByNameAsync(dto.BrandName);
                genericId = await GetGenericIdByNameAsync(dto.GenericName);
                formulationId = await GetFormulationIdByNameAsync(dto.FormulationName);
                classificationId = await GetClassificationIdByNameAsync(dto.ClassificationName);
                typeId = await GetTypeIdByNameAsync(dto.TypeName);

                // Validate required for medicine
                if (brandId == null)
                    throw new ArgumentException($"Brand '{dto.BrandName}' is required for medicine products.");
                if (genericId == null)
                    throw new ArgumentException($"Generic '{dto.GenericName}' is required for medicine products.");
                if (formulationId == null)
                    throw new ArgumentException($"Formulation '{dto.FormulationName}' is required for medicine products.");
                if (classificationId == null)
                    throw new ArgumentException($"Classification '{dto.ClassificationName}' is required for medicine products.");
                if (typeId == null)
                    throw new ArgumentException($"Type '{dto.TypeName}' is required for medicine products.");

                // Check for duplicates (excluding current product)
                if (await ProductExistsWithBrandAsync(dto.ProductName, brandId.Value, id))
                {
                    throw new InvalidOperationException(
                        $"A product with name '{dto.ProductName}' and brand '{dto.BrandName}' already exists.");
                }
            }
            else
            {
                // For non-medicine, brand is optional but resolve if provided
                if (!string.IsNullOrWhiteSpace(dto.BrandName))
                {
                    brandId = await GetBrandIdByNameAsync(dto.BrandName);
                }

                // Check for duplicates (excluding current product)
                if (brandId.HasValue)
                {
                    if (await ProductExistsWithBrandAsync(dto.ProductName, brandId.Value, id))
                    {
                        throw new InvalidOperationException(
                            $"A product with name '{dto.ProductName}' and brand '{dto.BrandName}' already exists.");
                    }
                }
                else
                {
                    if (await ProductExistsWithoutBrandAsync(dto.ProductName, id))
                    {
                        throw new InvalidOperationException(
                            $"A product with name '{dto.ProductName}' (without brand) already exists.");
                    }
                }
            }

            // Update the product with resolved IDs
            existing.ProductName = dto.ProductName;
            existing.CategoryID = categoryId.Value;
            existing.BrandID = brandId;
            existing.GenericID = genericId;
            existing.FormulationID = formulationId;
            existing.ClassificationID = classificationId;
            existing.TypeID = typeId;
            existing.SheetsPerBox = dto.SheetsPerBox;
            existing.PiecesPerSheet = dto.PiecesPerSheet;
            existing.BoxPrice = dto.BoxPrice;
            existing.SheetPrice = dto.SheetPrice;
            existing.PiecePrice = dto.PiecePrice;
            existing.ReorderLevel = dto.ReorderLevel;
            existing.Description = dto.Description;


            await _updateRepo.UpdateProductAsync(existing);
            return true;
        }
        public async Task<bool> ArchiveProductByIdAsync(int productId)
        {
            var product = await _context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product is null)
                return false;
            product.IsActive = false;
            var archiveProduct = await _updateRepo.UpdateProductAsync(product);
            if (!archiveProduct)
                return false;
            return archiveProduct;

        }
        #endregion

        #region Category
        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var validation = await ValidateCategoryNameAsync(dto.CategoryName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetCategoryByIdAsync(id);
            if (existing == null) return false;

            existing.CategoryName = dto.CategoryName;
            await _updateRepo.UpdateCategoryAsync(existing);
            return true;
        }

        public async Task<bool> ValidateCategoryNameAsync(string name, int categoryId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateCategoryNameAsync(name, categoryId);
            return validation;
        }

        public async Task<bool> ArchiveCategoryByIdAsync(int categoryId)
        {
            if (categoryId == 0) return false;
            var category = await _getRepo.GetCategoryByIdAsync(categoryId);
            if (category is null)
                return false;
            category.IsActive = false;
            await _updateRepo.UpdateCategoryAsync(category);
            return true;
        }

        public async Task<bool> RestoreCategoryByIdAsync(int categoryId) 
        {
          var category = await _getRepo.GetCategoryByIdAsync(categoryId);
          
          if(category is null)
            return false;

          category.IsActive = true;

          var restoration = await _updateRepo.UpdateCategoryAsync(category);

          if(!restoration)
            return false;

          return restoration;
        }

        #endregion

        #region Brand
        public async Task<bool> UpdateBrandAsync(int id, UpdateBrandDto dto)
        {
            var validation = await ValidateBrandNameAsync(dto.BrandName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetBrandByIdAsync(id);
            if (existing == null) return false;

            existing.BrandName = dto.BrandName;
            await _updateRepo.UpdateBrandAsync(existing);
            return true;
        }

        public async Task<bool> ValidateBrandNameAsync(string name, int brandId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateBrandNameAsync(name, brandId);
            return validation;
        }

        public async Task<bool> ArchiveBrandByIdAsync(int brandId)
        {
            var brand = await _getRepo.GetBrandByIdAsync(brandId);
            if (brand is null)
                return false;
            brand.IsActive = false;
            await _updateRepo.UpdateBrandAsync(brand);
            return true;
        }

        public async Task<bool> CategoryRestorationValidationAsync(int categoryId) 
        {
          var category = await _getRepo.GetCategoryByIdAsync(categoryId);
          if(category is null)
            return false;
          
          var duplication = await _updateRepo.CategoryRestorationValidationAsync(category.CategoryName);
          if(duplication)
            return false;
          return true;
        }

        #endregion

        #region Generic
        public async Task<bool> UpdateGenericAsync(int id, UpdateGenericDto dto)
        {
            var validation = await ValidateGenericNameAsync(dto.GenericName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetGenericByIdAsync(id);
            if (existing == null) return false;

            existing.GenericName = dto.GenericName;
            await _updateRepo.UpdateGenericAsync(existing);
            return true;
        }

        public async Task<bool> ValidateGenericNameAsync(string name, int genericId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateGenericNameAsync(name, genericId);
            return validation;
        }

        public async Task<bool> ArchiveGenericByIdAsync(int genericId)
        {
            var generic = await _getRepo.GetGenericByIdAsync(genericId);
            if (generic is null)
                return false;
            generic.IsActive = false;
            await _updateRepo.UpdateGenericAsync(generic);
            return true;
        }
        #endregion

        #region Formulation
        public async Task<bool> UpdateFormulationAsync(int id, UpdateFormulationDto dto)
        {
            var validation = await ValidateFormulationNameAsync(dto.FormulationName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetFormulationByIdAsync(id);
            if (existing == null) return false;

            existing.FormulationName = dto.FormulationName;
            await _updateRepo.UpdateFormulationAsync(existing);
            return true;
        }

        public async Task<bool> ValidateFormulationNameAsync(string name, int formulationId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateFormulationNameAsync(name, formulationId);
            return validation;
        }

        public async Task<bool> ArchiveFormulationByIdAsync(int formulationId)
        {
            var formulation = await _getRepo.GetFormulationByIdAsync(formulationId);
            if (formulation is null)
                return false;
            formulation.IsActive = false;
            await _updateRepo.UpdateFormulationAsync(formulation);
            return true;
        }
        #endregion

        #region Classification
        public async Task<bool> UpdateClassificationAsync(int id, UpdateClassificationDto dto)
        {
            var validation = await ValidateClassificationNameAsync(dto.ClassificationName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetClassificationByIdAsync(id);
            if (existing == null) return false;

            existing.ClassificationName = dto.ClassificationName;
            await _updateRepo.UpdateClassificationAsync(existing);
            return true;
        }

        public async Task<bool> ValidateClassificationNameAsync(string name, int classificationId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateClassificationNameAsync(name, classificationId);
            return validation;
        }

        public async Task<bool> ArchiveClassificationByIdAsync(int classificationId)
        {
            var classification = await _getRepo.GetClassificationByIdAsync(classificationId);
            if (classification is null)
                return false;
            classification.IsActive = false;
            await _updateRepo.UpdateClassificationAsync(classification);
            return true;
        }
        #endregion

        #region Type
        public async Task<bool> UpdateTypeModelAsync(int id, UpdateTypeModelDto dto)
        {
            var validation = await ValidateTypeNameAsync(dto.TypeName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetTypeModelByIdAsync(id);
            if (existing == null) return false;

            existing.TypeName = dto.TypeName;
            await _updateRepo.UpdateTypeModelAsync(existing);
            return true;
        }

        public async Task<bool> ValidateTypeNameAsync(string name, int typeId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateTypeNameAsync(name, typeId);
            return validation;
        }

        public async Task<bool> ArchiveTypeModelByIdAsync(int typeId)
        {
            var type = await _getRepo.GetTypeModelByIdAsync(typeId);
            if (type is null)
                return false;
            type.IsActive = false;
            await _updateRepo.UpdateTypeModelAsync(type);
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
            existing.ContactPerson = dto.ContactPerson;
            existing.ContactPersonNumber = dto.ContactPersonNumber;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.IsActive = dto.IsActive;

            await _updateRepo.UpdateSupplierAsync(existing);
            return true;
        }
        public async Task<bool> ArchiveSupplierByIdAsync(int supplierId)
        {
            var supplier = await _getRepo.GetSupplierByIdAsync(supplierId);
            if (supplier is null)
                return false;
            supplier.IsActive = false;

            return await _updateRepo.UpdateSupplierAsync(supplier);
        }
        public async Task<bool> AddProductsToSupplierAsync(AssignSupplierProductRequest request)
        {
            var supplier = await _getRepo.GetSupplierByIdAsync(request.SupplierId);
            if (supplier is null)
                return false;


            var productIds = request.ProductsToBeAdded.Select(p => p.ProductID).ToList();
            var products = await _context.Product
                .Where(p => productIds.Contains(p.ProductID))
                .ToListAsync();

            foreach (var product in products)
            {
                if (!supplier.Products.Any(sp => sp.ProductID == product.ProductID))
                {
                    supplier.Products.Add(product);
                }
            }
            _context.Supplier.Update(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Inventory
        public async Task<bool> UpdateInventoryAsync(int id, UpdateInventoryDto dto)
        {
            var existing = await _getRepo.GetInventoryByIdAsync(id);
            if (existing == null) return false;

            // Store before values for stock movement tracking
            var beforeBox = existing.BoxQuantity;
            var beforeSheet = existing.SheetQuantity;
            var beforePiece = existing.PieceQuantity;

            // Update basic properties
            existing.BatchNumber = dto.BatchNumber;
            existing.ManufacturingDate = dto.ManufacturingDate;
            existing.ExpirationDate = dto.ExpirationDate;
            existing.ReorderLevel = dto.ReorderLevel;
            existing.CostPrice = dto.CostPrice;
            existing.IsActive = dto.IsActive;

            // Update multi-unit quantities
            existing.BoxQuantity = dto.BoxQuantity;
            existing.SheetQuantity = dto.SheetQuantity;
            existing.PieceQuantity = dto.PieceQuantity;
            existing.SheetsPerBox = dto.SheetsPerBox;
            existing.PiecesPerSheet = dto.PiecesPerSheet;

            // Update multi-unit pricing
            existing.BoxPrice = dto.BoxPrice;
            existing.SheetPrice = dto.SheetPrice;
            existing.PiecePrice = dto.PiecePrice;

            var updated = await _updateRepo.UpdateInventoryAsync(existing);
            return true;
        }
        public async Task<bool> ArchiveInventoryByIdAsync(int inventoryId) 
        {
           if(inventoryId == 0)
             return false;

           var inventory = await _context.Inventory
             .FirstOrDefaultAsync(iv => iv.InventoryID == inventoryId);

           if(inventory is null)
             return false;

           inventory.IsActive = false;
           await _updateRepo.UpdateInventoryAsync(inventory);
           return true;
        }
        #endregion

        #region VAT
        public async Task<bool> UpdateVATAsync(int id, UpdateVATDto dto)
        {
            var validation = await ValidateVATNameAsync(dto.VATName, id);
            if (!validation)
                return false;

            var existing = await _getRepo.GetVATByIdAsync(id);
            if (existing == null) return false;

            existing.VATName = dto.VATName;
            existing.VATRate = dto.VATRate;
            existing.IsActive = dto.IsActive;
            await _updateRepo.UpdateVATAsync(existing);
            return true;
        }

        public async Task<bool> ValidateVATNameAsync(string name, int vatId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _updateRepo.ValidateVATNameAsync(name, vatId);
            return validation;
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
                var item = existing.PurchaseOrderItems.FirstOrDefault(poi => poi.Id == updatedItem.PurchaseOrderItemID);
                if (item != null)
                {
                    item.BoxQuantityOrdered = updatedItem.QuantityReceived;
                    item.BoxPrice = updatedItem.UnitCost;
                    item.TotalCost = item.BoxQuantityReceived * updatedItem.UnitCost;
                }
            }

            // Recalculate total amount
            existing.TotalAmount = existing.PurchaseOrderItems.Sum(poi => poi.TotalCost);

            await _updateRepo.UpdatePurchaseOrderAsync(existing);
            return true;

        }
        public async Task<MarkAsReceivedResult> MarkAsReceivedAsync(PurchaseOrderReceivedRequest request)
        {
            var purchaseOrder = await _getRepo.GetPurchaseOrderByIdAsync(request.PurchaseOrderId);
            purchaseOrder.ReceiptNumber = request.ReceiptNumber;
            if (purchaseOrder is null)
                return new MarkAsReceivedResult { Success = false, ProductName = "", Message = "Purchase Order does not exist."};

            foreach (var item in request.PurchaseOrderItems)
            {
                if (item.BoxQuantityReceived <= 0 && item.PieceQuantityReceived <= 0)
                    return new MarkAsReceivedResult {Success = false, Message = "Quantity Received is 0", ProductName = ""};
                var poItem = purchaseOrder.PurchaseOrderItems
                    .FirstOrDefault(po => po.Id == item.Id);
                if (poItem is null)
                    continue;

                var inventory = await _context.Inventory
                    .Include(i => i.Product)
                    .FirstOrDefaultAsync(i => i.ProductID == poItem.ProductID && i.BatchNumber == poItem.BatchNumber);

                var product = inventory?.Product ?? await _getRepo.GetProductByIdAsync(poItem.ProductID);
                if (product is null) continue;

                var sheetsPerBox = product?.SheetsPerBox ?? 0;
                var piecesPerSheet = product?.PiecesPerSheet ?? 0;
                var damagedPieces = item.DamagedPiecesCount;
                var expiredPieces = item.ExpiredPiecesCount;
                var defectPieces = item.DefectPiecesCount;
                var totalDeduction = damagedPieces + expiredPieces + defectPieces;

                if (inventory is null) 
                {
                    if (product.UnitMeasurement == "Multi-Level")
                    {
                        int computedPieceQty = (item.BoxQuantityReceived * sheetsPerBox * piecesPerSheet) - totalDeduction;
                        if(computedPieceQty <= 0) 
                          return new MarkAsReceivedResult {Success = false, ProductName = product.ProductName, 
                            Message = $"The total pieces of {product.ProductName} is less than 0"};
                        int computedSheetQty = computedPieceQty / piecesPerSheet;
                        int totalBoxes = computedSheetQty / sheetsPerBox;


                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = totalBoxes,
                            SheetQuantity = computedSheetQty,
                            PieceQuantity = computedPieceQty,
                            BatchNumber = item.BatchNumber,
                            ManufacturingDate = item.ManufacturingDate,
                            ExpirationDate = item.ExpirationDate,
                            ReorderLevel = product.ReorderLevel,
                            SheetsPerBox = sheetsPerBox,
                            PiecesPerSheet = piecesPerSheet,
                            BoxPrice = product.BoxPrice,
                            SheetPrice = product.SheetPrice,
                            PiecePrice = product.PiecePrice,
                            DateAdded = DateTime.UtcNow,
                            IsActive = true
                        };

                        await _postRepo.CreateInventoryAsync(newInventory);

                    }
                    else if (product.UnitMeasurement == "Dual-Level")
                    {
                        int computedPieceQty = (item.BoxQuantityReceived * product.PiecePerBox) - totalDeduction;
                        int totalBoxes = computedPieceQty / product.PiecePerBox;
                        if(computedPieceQty <= 0)
                            return new MarkAsReceivedResult
                            {
                                Success = false,
                                ProductName = product.ProductName,
                                Message = $"The total pieces of {product.ProductName} is less than 0"
                            };

                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = totalBoxes,
                            SheetQuantity = 0,
                            PieceQuantity = computedPieceQty,
                            BatchNumber = item.BatchNumber,
                            ManufacturingDate = item.ManufacturingDate,
                            ExpirationDate = item.ExpirationDate,
                            ReorderLevel = product.ReorderLevel,
                            SheetsPerBox = 0,
                            PiecesPerSheet = 0,
                            BoxPrice = product.BoxPrice,
                            SheetPrice = 0,
                            PiecePrice = product.PiecePrice,
                            DateAdded = DateTime.UtcNow,
                            IsActive = true,
                            PiecePerBox = product.PiecePerBox
                        };

                        await _postRepo.CreateInventoryAsync(newInventory);
                    }
                    else if(product.UnitMeasurement == "Single-Level") // Single-level
                    {
                        int actualPieces = item.PieceQuantityReceived - totalDeduction;
                        if (actualPieces <= damagedPieces)
                            return new MarkAsReceivedResult
                            {
                                Success = false,
                                ProductName = product.ProductName,
                                Message = "The total pieces of {product.ProductName} is less than 0"
                            };
                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = 0,
                            SheetQuantity = 0,
                            PieceQuantity = actualPieces,
                            BatchNumber = item.BatchNumber,
                            ManufacturingDate = item.ManufacturingDate,
                            ExpirationDate = item.ExpirationDate,
                            ReorderLevel = product.ReorderLevel,
                            SheetsPerBox = sheetsPerBox,
                            PiecesPerSheet = piecesPerSheet,
                            BoxPrice = 0,
                            SheetPrice = 0,
                            PiecePrice = product.PiecePrice,
                            DateAdded = DateTime.UtcNow,
                            IsActive = true
                        };

                        await _postRepo.CreateInventoryAsync(newInventory);
                    }
                }
                else
                {
                    // 🔹 Existing inventory → increase quantities
                    if (product.UnitMeasurement == "Multi-Level")
                    {
                        inventory.BoxQuantity += item.BoxQuantityReceived;
                        inventory.SheetQuantity += item.BoxQuantityReceived * sheetsPerBox;
                        inventory.PieceQuantity += item.BoxQuantityReceived * sheetsPerBox * piecesPerSheet;
                    }
                    else if (product.UnitMeasurement == "Dual-Level")
                    {
                        inventory.BoxQuantity += item.BoxQuantityReceived;
                        inventory.PieceQuantity += item.BoxQuantityReceived * sheetsPerBox;
                    }
                    else
                    {
                        inventory.PieceQuantity += item.PieceQuantityReceived;
                    }

                    _context.Inventory.Update(inventory);
                }
            }

            purchaseOrder.Status = "Received";
            purchaseOrder.DeliveryDate = DateTime.UtcNow;
            purchaseOrder.TotalAmount = request.TotalAmount;
            await _context.SaveChangesAsync();

            return new MarkAsReceivedResult {Message = "Purchase Order successfully received.", Success = true,};
        }

        public async Task<bool> ArchivePurchaseOrderByIdAsync(int id)
        {
            var purchaseOrder = await _getRepo.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder is null)
                return false;
            purchaseOrder.IsActive = false;
            _context.Update(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task LogStockMovementAsync(
        Inventory inventory,
        int beforeBox, int beforeSheet, int beforePiece,
        int changeBox, int changeSheet, int changePiece,
        string referenceNumber, string productName, int? employeeId)
        {
            var stockMovement = new StockMovement
            {
                InventoryID = inventory.InventoryID,
                MovementType = "Received PO",
                BoxQuantityBefore = beforeBox,
                SheetQuantityBefore = beforeSheet,
                PieceQuantityBefore = beforePiece,
                BoxQuantityChanged = changeBox,
                SheetQuantityChanged = changeSheet,
                PieceQuantityChanged = changePiece,
                BoxQuantityAfter = inventory.BoxQuantity,
                SheetQuantityAfter = inventory.SheetQuantity,
                PieceQuantityAfter = inventory.PieceQuantity,
                ReferenceNumber = referenceNumber ?? $"PO-{referenceNumber}",
                Notes = $"New stock received for product {productName}.",
                EmployeeID = employeeId,
            };

            await _context.StockMovement.AddAsync(stockMovement);
        }
        #endregion

        #region BackUp Database

        public async Task<bool> UpdateBackupSettingsAsync(BackupSettingsDto settings)
        {
            try
            {
                // Repository handles all validation
                return await _updateRepo.UpdateBackupSettingsAsync(settings);
            }
            catch (Exception ex)
            {
                // Log if you have logger
                return false;
            }
        }

        #endregion
    }
}
