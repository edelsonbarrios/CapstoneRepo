using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Service.PostService
{
    public class PostService : IPostServiceContracts
    {
        private readonly IPostRepositoryContracts _postRepo;
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IUpdateRepositoryContracts _updateRepo;
        private readonly PharmacyContext _context;

        public PostService(IPostRepositoryContracts postRepo,
            IGetRepositoryContracts getRepo,
            IUpdateRepositoryContracts updateRepo,
            PharmacyContext context)
        {
            _postRepo = postRepo;
            _getRepo = getRepo;         
            _updateRepo = updateRepo;
            _context = context;
        }

        #region Account
        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var enteredPassword = AesHelper.Encrypt(request.Password);
            request.Password = enteredPassword;
            var token = await _postRepo.AuthenticationAsync(request);
            return token;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountRequest request)
        {
            var encryptedPassword = AesHelper.Encrypt(request.Password);
            var newAccount = new Account()
            {
                Username = request.Username,
                EncryptedPassword = encryptedPassword,
                EmployeeId = request.EmployeeId
            };

            var createAccount = await _postRepo.CreateAccountAsync(newAccount);
            if (!createAccount)
                return false;
            return createAccount;
        }
        #endregion

        #region Employee
        public async Task<int> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (request is null)
                return 0;

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RolePosition == request.RolePosition);

            if (role is null)
            {
                return 0;
            }

            var newEmployee = new Employee
            {
                EmployeeCode = request.EmployeeCode,
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                Age = request.Age,
                BirthDate = request.BirthDate,
                CivilStatus = request.CivilStatus,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                RoleId = role.Id,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                ArchivedBy = request.ArchivedBy,
                ArchivedAt = request.ArchivedAt,
                UpdatedBy = request.UpdatedBy,
                UpdatedAt = request.UpdatedAt
            };

            var employeeId = await _postRepo.CreateEmployeeAsync(newEmployee);
            if (employeeId == 0)
                return 0;

            var employeeHistory = new EmployeeHistory()
            {
                EmployeeId = employeeId,
                ActionPerformed = "Created",
                PerformedBy = request.CreatedBy,
                PerformedAt = DateTime.UtcNow,
            };

            var createEmployeeHistory = await _postRepo.CreateEmployeeHistoryAsync(employeeHistory);
            if (!createEmployeeHistory)
                return 0;

            return employeeId;
        }
        #endregion

        #region EmployeeHistory

        #endregion

        #region Sales
        public async Task<SalesTransactionDto> CreateSalesTransactionAsync(CreateSalesTransactionDto dto)
        {
            try
            {
                if (dto == null || dto.Items == null || !dto.Items.Any())
                    throw new ArgumentException("Sales transaction must have at least one item");

                // Create the sales transaction entity
                var transaction = new SalesTransaction
                {
                    TransactionDate = DateTime.UtcNow,
                    InvoiceNumber = dto.InvoiceNumber,
                    PaymentMethod = dto.PaymentMethod,
                    AmountPaid = dto.AmountPaid,
                    ChangeAmount = dto.ChangeAmount,
                    DiscountId = dto.DiscountId,
                    EmployeeID = dto.EmployeeID,
                    VATId = dto.VATId,
                    DiscountAmount = dto.DiscountAmount,
                    VATAmount = dto.VATAmount,
                    Items = dto.Items.Select(i => new SalesItem
                    {
                        ProductId = i.ProductId,
                        UnitType = i.UnitType,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.Quantity * i.UnitPrice
                    }).ToList(),
                    TotalAmount = dto.TotalAmount
                };

                // Repository will handle inventory deduction and stock movements
                var createdTransaction = await _postRepo.CreateSalesTransactionAsync(transaction);

                var validateInventory = await _updateRepo.ValidateInventoryStocksAsync();

                // Return DTO
                return new SalesTransactionDto
                {
                    Id = createdTransaction.Id,
                    TransactionDate = createdTransaction.TransactionDate,
                    InvoiceNumber = createdTransaction.InvoiceNumber,
                    TotalAmount = createdTransaction.TotalAmount,
                    DiscountAmount = createdTransaction.DiscountAmount,
                    VATAmount = createdTransaction.VATAmount,
                    NetAmount = createdTransaction.NetAmount,
                    PaymentMethod = createdTransaction.PaymentMethod,
                    AmountPaid = createdTransaction.AmountPaid,
                    ChangeAmount = createdTransaction.ChangeAmount,
                    DiscountId = createdTransaction.DiscountId,
                    EmployeeID = createdTransaction.EmployeeID,
                    VATId = createdTransaction.VATId,
                    DiscountName = createdTransaction.Discount?.DiscountName ?? string.Empty,
                    EmployeeName = createdTransaction.Employee != null ? $"{createdTransaction.Employee.FirstName} {createdTransaction.Employee.LastName}" : string.Empty,
                    VATRate = createdTransaction.VAT?.VATRate,
                    Items = createdTransaction.Items.Select(i => new SalesItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        UnitType = i.UnitType,
                        ProductName = i.Product?.ProductName ?? string.Empty,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.Subtotal
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating sales transaction: {ex.Message}", ex);
            }
        }

        public async Task<SalesItemDto> CreateSalesItemAsync(CreateSalesItemDto dto)
        {
            try
            {
                var salesItem = new SalesItem
                {
                    SalesTransactionId = dto.SalesTransactionId,
                    ProductId = dto.ProductId,
                    UnitType = dto.UnitType,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    Subtotal = dto.Quantity * dto.UnitPrice
                };

                var created = await _postRepo.CreateSalesItemAsync(salesItem);

                return new SalesItemDto
                {
                    Id = created.Id,
                    ProductId = created.ProductId,
                    ProductName = created.Product?.ProductName ?? string.Empty,
                    UnitType = created.UnitType,
                    Quantity = created.Quantity,
                    UnitPrice = created.UnitPrice,
                    Subtotal = created.Subtotal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating sales item: {ex.Message}", ex);
            }
        }
        #endregion

        #region Discount
        public async Task<bool> CreateDiscountAsync(CreateDiscountDto dto)
        {
            var validation = await _postRepo.ValidateDiscountNameAsync(dto.DiscountName);
            if (!validation)
                return false;
            var discount = new Discount
            {
                DiscountName = dto.DiscountName,
                DiscountRate = dto.DiscountRate

            };

            var result = await _postRepo.CreateDiscountAsync(discount);
            return true;
        }
        public async Task<bool> ValidateDiscountNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            var validation = await _postRepo.ValidateDiscountNameAsync(name);
            if (!validation)
                return false;
            return validation;
        }
        #endregion

        #region product helper
        private async Task<Product?> GetProductByNameAsync(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return null;

            return await _context.Product
                .FirstOrDefaultAsync(p => p.ProductName.ToLower() == productName.ToLower());
        }
        private async Task<bool> ProductExistsWithoutBrandAsync(string productName)
        {
            return await _context.Product
                .AnyAsync(p => p.ProductName == productName && p.BrandID == null);
        }
        private async Task<bool> ProductExistsWithBrandAsync(string productName, int brandId)
        {
            return await _context.Product
                .AnyAsync(p => p.ProductName.ToLower() == productName.ToLower()
                            && p.BrandID == brandId);
        }

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

        #region Product crud

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            // Resolve category first to determine if it's medicine
            var categoryId = await GetCategoryIdByNameAsync(dto.CategoryName);
            if (categoryId == null)
                throw new ArgumentException($"Category '{dto.CategoryName}' not found.");

            // Get category to check if it's medicine
            var category = await _context.Category.FindAsync(categoryId.Value);
            if (category == null)
                throw new ArgumentException($"Category not found in database.");

            bool isMedicine = category.CategoryName.Equals("Medicine", StringComparison.OrdinalIgnoreCase);

            // Medicine-specific fields - only resolve and validate if category is Medicine
            int? brandId = null;
            int? genericId = null;
            int? formulationId = null;
            int? classificationId = null;
            int? typeId = null;

            if (isMedicine)
            {
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

                // Check if medicine product with same name and brand already exists
                if (await ProductExistsWithBrandAsync(dto.ProductName, brandId.Value))
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

                // For non-medicine, check duplicate based on whether brand was provided
                if (brandId.HasValue)
                {
                    // If brand provided, check name + brand combination
                    if (await ProductExistsWithBrandAsync(dto.ProductName, brandId.Value))
                    {
                        throw new InvalidOperationException(
                            $"A product with name '{dto.ProductName}' and brand '{dto.BrandName}' already exists.");
                    }
                }
                else
                {
                    // If no brand, check if product name exists without brand
                    if (await ProductExistsWithoutBrandAsync(dto.ProductName))
                    {
                        throw new InvalidOperationException(
                            $"A product with name '{dto.ProductName}' (without brand) already exists.");
                    }
                }
            }

            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryID = categoryId.Value,
                BrandID = brandId,
                GenericID = genericId,
                FormulationID = formulationId,
                ClassificationID = classificationId,
                TypeID = typeId,
                IsActive = true,
                SheetsPerBox = dto.SheetsPerBox,
                PiecesPerSheet = dto.PiecesPerSheet,
                BoxPrice = dto.BoxPrice,
                SheetPrice = dto.SheetPrice,
                PiecePrice = dto.PiecePrice,
                ReorderLevel = dto.ReorderLevel,
                UnitMeasurement = dto.UnitMeasurement,
                Description = dto.Description,
                DateCreated = DateTime.UtcNow,
                PiecePerBox = dto.PiecePerBox
            };

            var created = await _postRepo.CreateProductAsync(product);

            // Return complete ProductDto with all properties populated
            var fullProduct = await _getRepo.GetProductByIdAsync(created.ProductID);
            return new ProductDto
            {
                ProductID = fullProduct.ProductID,
                ProductName = fullProduct.ProductName,
                CategoryID = fullProduct.CategoryID,
                BrandID = fullProduct.BrandID,
                GenericID = fullProduct.GenericID,
                FormulationID = fullProduct.FormulationID,
                ClassificationID = fullProduct.ClassificationID,
                TypeID = fullProduct.TypeID,
                IsActive = fullProduct.IsActive,
                DateCreated = fullProduct.DateCreated,

                // Initialize stock values (will be 0 for new products)
                ExpiredStock = 0,
                NearExpiryStock = 0,
                IsCriticalStock = false,
                HasExpiredStock = false,

                // Related entity names
                CategoryName = fullProduct.Category?.CategoryName ?? string.Empty,
                BrandName = fullProduct.Brand?.BrandName ?? string.Empty,
                GenericName = fullProduct.Generic?.GenericName ?? string.Empty,
                FormulationName = fullProduct.Formulation?.FormulationName ?? string.Empty,
                ClassificationName = fullProduct.Classification?.ClassificationName ?? string.Empty,
                TypeName = fullProduct.TypeModel?.TypeName ?? string.Empty,
            };
        }
        #endregion

        #region Category
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName
            };

            var created = await _postRepo.CreateCategoryAsync(category);

            return new CategoryDto
            {
                CategoryID = created.CategoryID,
                CategoryName = created.CategoryName
            };
        }
        #endregion

        #region Brand
        public async Task<BrandDto> CreateBrandAsync(CreateBrandDto dto)
        {
            var validation = await _postRepo.ValidateBrandNameAsync(dto.BrandName);
            if (!validation)
                return null;

            var brand = new Brand
            {
                BrandName = dto.BrandName
            };
            var created = await _postRepo.CreateBrandAsync(brand);
            return new BrandDto
            {
                BrandID = created.BrandID,
                BrandName = created.BrandName
            };
        }
        #endregion

        #region Generic
        public async Task<GenericDto> CreateGenericAsync(CreateGenericDto dto)
        {
            var validation = await _postRepo.ValidateGenericNameAsync(dto.GenericName);
            if (!validation)
                return null;

            var generic = new Generic
            {
                GenericName = dto.GenericName
            };
            var created = await _postRepo.CreateGenericAsync(generic);
            return new GenericDto
            {
                GenericID = created.GenericID,
                GenericName = created.GenericName
            };
        }
        #endregion

        #region Formulation
        public async Task<FormulationDto> CreateFormulationAsync(CreateFormulationDto dto)
        {
            var validation = await _postRepo.ValidateFormulationNameAsync(dto.FormulationName);
            if (!validation)
                return null;

            var formulation = new Formulation
            {
                FormulationName = dto.FormulationName
            };
            var created = await _postRepo.CreateFormulationAsync(formulation);
            return new FormulationDto
            {
                FormulationID = created.FormulationID,
                FormulationName = created.FormulationName
            };
        }
        #endregion

        #region Classification
        public async Task<ClassificationDto> CreateClassificationAsync(CreateClassificationDto dto)
        {
            var validation = await _postRepo.ValidateClassificationNameAsync(dto.ClassificationName);
            if (!validation)
                return null;

            var classification = new Classification
            {
                ClassificationName = dto.ClassificationName
            };
            var created = await _postRepo.CreateClassificationAsync(classification);
            return new ClassificationDto
            {
                ClassificationID = created.ClassificationID,
                ClassificationName = created.ClassificationName
            };
        }
        #endregion

        #region Type
        public async Task<TypeModelDto> CreateTypeModelAsync(CreateTypeModelDto dto)
        {
            var validation = await _postRepo.ValidateTypeNameAsync(dto.TypeName);
            if (!validation)
                return null;

            var type = new TypeModel
            {
                TypeName = dto.TypeName
            };
            var created = await _postRepo.CreateTypeModelAsync(type);
            return new TypeModelDto
            {
                TypeID = created.TypeID,
                TypeName = created.TypeName
            };
        }
        #endregion

        #region Supplier
        public async Task<bool> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                SupplierName = dto.SupplierName,
                ContactNumber = dto.ContactNumber,
                ContactPerson = dto.ContactPerson,
                ContactPersonNumber = dto.ContactPersonNumber,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = true,
                Products = new List<Product>()
            };

            var created = await _postRepo.CreateSupplierAsync(supplier);

            return created;
        }
        #endregion

        #region Inventory
        public async Task<bool> CreateInventoryAsync(CreateInventoryDto dto)
        {
            try
            {

                var product = await GetProductByNameAsync(dto.ProductName);
                if (product == null)
                    throw new ArgumentException($"Product '{dto.ProductName}' not found.");

                
                var existingInventory = await _context.Inventory
                    .FirstOrDefaultAsync(i => i.ProductID == product.ProductID &&
                                             i.BatchNumber == dto.BatchNumber &&
                                             i.IsActive);

                if (existingInventory != null)
                {
                    existingInventory.BoxQuantity += dto.BoxQuantity;
                    existingInventory.SheetQuantity += dto.SheetQuantity;
                    existingInventory.PieceQuantity += dto.PieceQuantity;
                    existingInventory.ExpirationDate = dto.ExpirationDate;
                    existingInventory.ManufacturingDate = dto.ManufacturingDate;
                    existingInventory.BoxPrice = dto.BoxPrice;
                    existingInventory.SheetPrice = dto.SheetPrice;
                    existingInventory.PiecePrice = dto.PiecePrice;
                    existingInventory.CostPrice = dto.CostPrice;


                    await _updateRepo.UpdateInventoryAsync(existingInventory);

                    var movement = new CreateStockMovementDto
                    {
                        InventoryID = existingInventory.InventoryID,
                        MovementType = "Stock In",
                        BoxQuantityChanged = dto.BoxQuantity,
                        SheetQuantityChanged = dto.SheetQuantity,
                        PieceQuantityChanged = dto.PieceQuantity,
                        ReferenceNumber = dto.ReferenceNumber,
                        Notes = dto.Notes ?? $"Stock addition to batch {dto.BatchNumber}"
                    };

                    await CreateStockMovementAsync(movement);
                    return true;
                }


                Inventory inventory;

                if (product.UnitMeasurement == "Multi-Level")
                {
                    
                    inventory = new Inventory
                    {
                        ProductID = product.ProductID,
                        BatchNumber = dto.BatchNumber,
                        ManufacturingDate = dto.ManufacturingDate,
                        ExpirationDate = dto.ExpirationDate,
                        BoxQuantity = dto.BoxQuantity,
                        SheetQuantity = product.SheetsPerBox,
                        PieceQuantity = product.PiecesPerSheet,
                        SheetsPerBox = dto.SheetsPerBox ?? product.SheetsPerBox,
                        PiecesPerSheet = dto.PiecesPerSheet ?? product.PiecesPerSheet,
                        ReorderLevel = dto.ReorderLevel,
                        BoxPrice = dto.BoxPrice,
                        SheetPrice = dto.SheetPrice,
                        PiecePrice = dto.PiecePrice,
                        CostPrice = dto.CostPrice,
                        IsActive = true,
                        DateAdded = DateTime.UtcNow
                    };
                }
                else if (product.UnitMeasurement == "Dual-Level")
                {
                    inventory = new Inventory
                    {
                        ProductID = product.ProductID,
                        BatchNumber = dto.BatchNumber,
                        ManufacturingDate = dto.ManufacturingDate,
                        ExpirationDate = dto.ExpirationDate,
                        BoxQuantity = dto.BoxQuantity,
                        SheetQuantity = 0, // no sheets in dual-level
                        PieceQuantity = product.PiecePerBox,
                        PiecePerBox = dto.PiecePerBox ?? product.PiecePerBox,
                        ReorderLevel = dto.ReorderLevel,
                        BoxPrice = dto.BoxPrice,
                        PiecePrice = dto.PiecePrice,
                        CostPrice = dto.CostPrice,
                        IsActive = true,
                        DateAdded = DateTime.UtcNow
                    };
                }
                else
                {
                    // 🔸 Single-Level fallback (Box only)
                    inventory = new Inventory
                    {
                        ProductID = product.ProductID,
                        BatchNumber = dto.BatchNumber,
                        ManufacturingDate = dto.ManufacturingDate,
                        ExpirationDate = dto.ExpirationDate,
                        BoxQuantity = dto.BoxQuantity,
                        PieceQuantity = dto.PieceQuantity,
                        PiecePerBox = dto.PiecePerBox ?? product.PiecePerBox,
                        BoxPrice = dto.BoxPrice,
                        PiecePrice = dto.PiecePrice,
                        CostPrice = dto.CostPrice,
                        IsActive = true,
                        DateAdded = DateTime.UtcNow
                    };

                }
                var created = await _postRepo.CreateInventoryAsync(inventory);

                if (created)
                {
                    var movement = new CreateStockMovementDto
                    {
                        InventoryID = inventory.InventoryID,
                        MovementType = "Stock In",
                        BoxQuantityChanged = dto.BoxQuantity,
                        SheetQuantityChanged = dto.SheetQuantity,
                        PieceQuantityChanged = dto.PieceQuantity,
                        ReferenceNumber = dto.ReferenceNumber,
                        Notes = dto.Notes ?? $"New batch {dto.BatchNumber} created."
                    };

                    await CreateStockMovementAsync(movement);
                }

                return created;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating inventory: {ex.Message}", ex);
            }
        }


        public async Task<bool> AdjustInventoryStockAsync(StockAdjustmentDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get current inventory
                var inventory = await _getRepo.GetInventoryByIdAsync(dto.InventoryID);
                if (inventory == null)
                    throw new ArgumentException($"Inventory with ID {dto.InventoryID} not found");

                // Validate movement type
                var validTypes = new[] { "Damaged", "Expired", "Loss", "Found", "Count Correction", "Transfer", "Return" };
                if (!validTypes.Contains(dto.MovementType, StringComparer.OrdinalIgnoreCase))
                    throw new ArgumentException($"Invalid movement type. Valid types: {string.Join(", ", validTypes)}");

                // Calculate new quantities
                var newBoxQty = inventory.BoxQuantity + dto.BoxQuantityAdjustment;
                var newSheetQty = inventory.SheetQuantity + dto.SheetQuantityAdjustment;
                var newPieceQty = inventory.PieceQuantity + dto.PieceQuantityAdjustment;

                // Strict validation - no negatives
                if (newBoxQty < 0 || newSheetQty < 0 || newPieceQty < 0)
                    throw new ArgumentException("Adjustment would result in negative quantities");

                // Create stock movement first (using your existing CreateStockMovementDto)
                var stockMovement = new CreateStockMovementDto
                {
                    InventoryID = dto.InventoryID,
                    MovementType = dto.MovementType,
                    BoxQuantityChanged = dto.BoxQuantityAdjustment,
                    SheetQuantityChanged = dto.SheetQuantityAdjustment,
                    PieceQuantityChanged = dto.PieceQuantityAdjustment,
                    ReferenceNumber = dto.ReferenceNumber,
                    Notes = dto.Notes,
                };

                await CreateStockMovementAsync(stockMovement);

                // Update inventory quantities
                inventory.BoxQuantity = newBoxQty;
                inventory.SheetQuantity = newSheetQty;
                inventory.PieceQuantity = newPieceQty;

                await _updateRepo.UpdateInventoryAsync(inventory);
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error adjusting stock: {ex.Message}", ex);
            }
        }
        #endregion

        #region VAT
        public async Task<bool> CreateVATAsync(CreateVATDto dto)
        {
            var validation = await _postRepo.VatNameValidationAsync(dto.VATName);
            if (!validation)
                return false;
            var vat = new VAT
            {
                VATName = dto.VATName,
                VATRate = dto.VATRate,
                IsActive = true
            };

            var created = await _postRepo.CreateVATAsync(vat);

            return true;
        }
        public async Task<bool> VatNameValidationAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var validation = await _postRepo.VatNameValidationAsync(name);
            return validation;
        }
        #endregion

        #region PurchaseOrder
        public async Task<bool> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
        {
            var supplier = await _context.Supplier.FirstOrDefaultAsync(s => s.SupplierName == dto.SupplierName);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.LastName == dto.EmployeeLastName);

            if (supplier == null || employee == null)
                return false;

            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderNumber = dto.PurchaseOrderNumber,
                OrderDate = DateTime.UtcNow,
                DeliveryDate = dto.DeliveryDate,
                EmployeeID = employee.Id,
                SupplierID = supplier.Id,
                Notes = dto.Notes,
                Status = "Pending",
                PurchaseOrderItems = dto.PurchaseOrderItems.Select(poi => new PurchaseOrderItem
                {
                    ProductID = poi.ProductID,
                    BoxQuantityOrdered = poi.BoxQuantityOrdered,
                    SheetQuantityOrdered = poi.SheetQuantityOrdered,
                    PieceQuantityOrdered = poi.PieceQuantityOrdered,
                    BoxPrice = poi.BoxPrice,
                    SheetPrice = poi.SheetPrice,
                    PiecePrice = poi.PiecePrice,
                    TotalCost = poi.TotalCost,
                    BatchNumber = poi.BatchNumber,
                    ManufacturingDate = poi.ManufacturingDate,
                    ExpirationDate = poi.ExpirationDate,
                    PoUnitType = poi.PoUnitType,
                }).ToList()
            };

            
            purchaseOrder.TotalAmount = purchaseOrder.PurchaseOrderItems.Sum(poi => poi.TotalCost);

            await _context.PurchaseOrder.AddAsync(purchaseOrder);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<string> GenerateNewPurchaseOrderNumberAsync()
        {
            var newPurchaseOrderNumber = await _postRepo.GenerateNewPurchaseOrderNumberAsync();
            if (string.IsNullOrWhiteSpace(newPurchaseOrderNumber))
            {
                return null;
            }
            return newPurchaseOrderNumber;
        }
        #endregion

        #region StockMovement
        public async Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto dto)
        {
            // Get the current inventory to calculate before/after quantities
            var inventory = await _getRepo.GetInventoryByIdAsync(dto.InventoryID);
            if (inventory == null)
                throw new ArgumentException("Inventory not found");

            var stockMovement = new StockMovement
            {
                InventoryID = dto.InventoryID,
                MovementType = dto.MovementType,
                BoxQuantityChanged = dto.BoxQuantityChanged,
                SheetQuantityChanged = dto.SheetQuantityChanged,
                PieceQuantityChanged = dto.PieceQuantityChanged,
                BoxQuantityBefore = inventory.BoxQuantity,
                SheetQuantityBefore = inventory.SheetQuantity,
                PieceQuantityBefore = inventory.PieceQuantity,
                BoxQuantityAfter = inventory.BoxQuantity + dto.BoxQuantityChanged,
                SheetQuantityAfter = inventory.SheetQuantity + dto.SheetQuantityChanged,
                PieceQuantityAfter = inventory.PieceQuantity + dto.PieceQuantityChanged,
                ReferenceNumber = dto.ReferenceNumber,
                Notes = dto.Notes,
                EmployeeID = dto.EmployeeID
            };

            var created = await _postRepo.CreateStockMovementAsync(stockMovement);

            // Update the inventory quantities
            inventory.BoxQuantity = stockMovement.BoxQuantityAfter;
            inventory.SheetQuantity = stockMovement.SheetQuantityAfter;
            inventory.PieceQuantity = stockMovement.PieceQuantityAfter;
            await _updateRepo.UpdateInventoryAsync(inventory);

            return new StockMovementDto
            {
                StockMovementID = created.StockMovementID,
                InventoryID = created.InventoryID,
                ProductName = inventory.Product?.ProductName ?? string.Empty,
                BatchNumber = inventory.BatchNumber,
                MovementType = created.MovementType,
                BoxQuantityChanged = created.BoxQuantityChanged,
                SheetQuantityChanged = created.SheetQuantityChanged,
                PieceQuantityChanged = created.PieceQuantityChanged,
                BoxQuantityBefore = created.BoxQuantityBefore,
                SheetQuantityBefore = created.SheetQuantityBefore,
                PieceQuantityBefore = created.PieceQuantityBefore,
                BoxQuantityAfter = created.BoxQuantityAfter,
                SheetQuantityAfter = created.SheetQuantityAfter,
                PieceQuantityAfter = created.PieceQuantityAfter,
                ReferenceNumber = created.ReferenceNumber,
                Notes = created.Notes,
                MovementDate = created.MovementDate,
                EmployeeID = created.EmployeeID
            };
        }
        #endregion

    }
}
