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

namespace PharmacyBackend.Service.UpdateService
{
    public class UpdateService : IUpdateServiceContracts
    {
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IPostRepositoryContracts _postRepo;
        private readonly PharmacyContext _context;

        public UpdateService(IUpdateRepositoryContracts updateRepo,
            IGetRepositoryContracts getRepo,
            IPostRepositoryContracts postRepo,
            PharmacyContext context)
        {
            _updateRepo = updateRepo;
            _getRepo = getRepo;
            _postRepo = postRepo;
            _context = context;
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
            var existing = await _getRepo.GetDiscountByIdAsync(id);
            if (existing == null) return false;

            existing.DiscountName = dto.DiscountName;
            existing.DiscountRate = dto.DiscountRate;
            existing.IsActive = dto.IsActive;

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

        #region Product Helper
        private async Task<int?> GetCategoryIdByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;

            var category = await _context.Category
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
            return category?.CategoryID;
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

            // Resolve names to IDs using the same helper methods
            var categoryId = await GetCategoryIdByNameAsync(dto.CategoryName);
            var brandId = await GetBrandIdByNameAsync(dto.BrandName);
            var genericId = await GetGenericIdByNameAsync(dto.GenericName);
            var formulationId = await GetFormulationIdByNameAsync(dto.FormulationName);
            var classificationId = await GetClassificationIdByNameAsync(dto.ClassificationName);
            var typeId = await GetTypeIdByNameAsync(dto.TypeName);

            // Validate that all required entities exist
            if (categoryId == null)
                throw new ArgumentException($"Category '{dto.CategoryName}' not found.");
            if (brandId == null)
                throw new ArgumentException($"Brand '{dto.BrandName}' not found.");
            if (genericId == null)
                throw new ArgumentException($"Generic '{dto.GenericName}' not found.");
            if (formulationId == null)
                throw new ArgumentException($"Formulation '{dto.FormulationName}' not found.");
            if (classificationId == null)
                throw new ArgumentException($"Classification '{dto.ClassificationName}' not found.");
            if (typeId == null)
                throw new ArgumentException($"Type '{dto.TypeName}' not found.");

            // Update the product with resolved IDs
            existing.ProductName = dto.ProductName;
            existing.CategoryID = categoryId.Value;
            existing.BrandID = brandId.Value;
            existing.GenericID = genericId.Value;
            existing.FormulationID = formulationId.Value;
            existing.ClassificationID = classificationId.Value;
            existing.TypeID = typeId.Value;
            existing.SheetsPerBox = dto.SheetsPerBox;
            existing.PiecesPerSheet = dto.PiecesPerSheet;
            existing.BoxPrice = dto.BoxPrice;
            existing.SheetPrice = dto.SheetPrice;
            existing.PiecePrice = dto.PiecePrice;
            existing.ReorderLevel = dto.ReorderLevel;


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
        public async Task<bool> MarkAsReceivedAsync(PurchaseOrderReceivedRequest request)
        {
            var purchaseOrder = await _getRepo.GetPurchaseOrderByIdAsync(request.PurchaseOrderId);
            if (purchaseOrder is null)
                return false;

            foreach (var item in request.PurchaseOrderItems)
            {
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

                // If inventory doesn’t exist → create new
                if (inventory is null)
                {
                    if (product.UnitMeasurement == "Multi-Level")
                    {
                        var computedSheetQty = item.BoxQuantityReceived * sheetsPerBox;
                        var computedPieceQty = computedSheetQty * piecesPerSheet;

                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = item.BoxQuantityReceived,
                            SheetQuantity = product.SheetsPerBox,
                            PieceQuantity = product.PiecesPerSheet,
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

                        await LogStockMovementAsync(newInventory, 0, 0, 0,
                            item.BoxQuantityReceived, computedSheetQty, computedPieceQty,
                            purchaseOrder.PurchaseOrderNumber, product.ProductName, request.EmployeeId);
                    }
                    else if (product.UnitMeasurement == "Dual-Level")
                    {
                        var computedPieceQty = item.BoxQuantityReceived * sheetsPerBox;

                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = item.BoxQuantityReceived,
                            SheetQuantity = 0,
                            PieceQuantity = product.PiecePerBox,
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

                        await LogStockMovementAsync(newInventory, 0, 0, 0,
                            item.BoxQuantityReceived, 0, computedPieceQty,
                            purchaseOrder.PurchaseOrderNumber, product.ProductName, request.EmployeeId);
                    }
                    else // Single-level
                    {
                        var newInventory = new Inventory
                        {
                            ProductID = item.ProductID,
                            BoxQuantity = 0,
                            SheetQuantity = 0,
                            PieceQuantity = item.BoxQuantityReceived,
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

                        await LogStockMovementAsync(newInventory, 0, 0, 0,
                            0, 0, item.BoxQuantityReceived,
                            purchaseOrder.PurchaseOrderNumber, product.ProductName, request.EmployeeId);
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
                        inventory.PieceQuantity += item.BoxQuantityReceived;
                    }

                    _context.Inventory.Update(inventory);

                    await LogStockMovementAsync(
                        inventory,
                        inventory.BoxQuantity,
                        inventory.SheetQuantity,
                        inventory.PieceQuantity,
                        item.BoxQuantityReceived,
                        item.BoxQuantityReceived * sheetsPerBox,
                        item.BoxQuantityReceived * sheetsPerBox * piecesPerSheet,
                        purchaseOrder.PurchaseOrderNumber,
                        product.ProductName,
                        request.EmployeeId);
                }
            }

            purchaseOrder.Status = "Received";
            purchaseOrder.DeliveryDate = DateTime.UtcNow;
            purchaseOrder.TotalAmount = request.TotalAmount;
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
    }
}
