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
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return null;

            var transaction = new SalesTransaction
            {
                TransactionDate = DateTime.Now,
                InvoiceNumber = dto.InvoiceNumber,
                DiscountId = dto.DiscountId,
                DiscountAmount = 0,
                Items = dto.Items.Select(i => new SalesItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };

            transaction.TotalAmount = transaction.Items.Sum(i => i.Subtotal);
            transaction.NetAmount = transaction.TotalAmount - transaction.DiscountAmount;

            var created = await _postRepo.CreateSalesTransactionAsync(transaction);

            return new SalesTransactionDto
            {
                Id = created.Id,
                InvoiceNumber = created.InvoiceNumber,
                TransactionDate = created.TransactionDate,
                TotalAmount = created.TotalAmount,
                NetAmount = created.NetAmount
            };
        }
        #endregion

        #region Discount
        public async Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto)
        {
            var discount = new Discount
            {
                DiscountName = dto.DiscountName,
                DiscountRate = dto.DiscountRate
            };

            var result = await _postRepo.CreateDiscountAsync(discount);
            return new DiscountDto
            {
                Id = result.Id,
                DiscountName = result.DiscountName,
                DiscountRate = result.DiscountRate
            };
        }
        #endregion

        #region Product
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryID = dto.CategoryID,
                BrandID = dto.BrandID,
                GenericID = dto.GenericID,
                FormulationID = dto.FormulationID,
                ClassificationID = dto.ClassificationID,
                TypeID = dto.TypeID,
                SupplierID = dto.SupplierID,
                Price = dto.Price,
                IsActive = true
            };

            var created = await _postRepo.CreateProductAsync(product);

            return new ProductDto
            {
                ProductID = created.ProductID,
                ProductName = created.ProductName,
                CategoryID = created.CategoryID,
                BrandID = created.BrandID,
                GenericID = created.GenericID,
                FormulationID = created.FormulationID,
                ClassificationID = created.ClassificationID,
                TypeID = created.TypeID,
                SupplierID = created.SupplierID,
                Price = created.Price,
                IsActive = created.IsActive
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
        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                SupplierName = dto.SupplierName,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = true
            };

            var created = await _postRepo.CreateSupplierAsync(supplier);

            return new SupplierDto
            {
                SupplierID = created.SupplierID,
                SupplierName = created.SupplierName,
                ContactNumber = created.ContactNumber,
                Email = created.Email,
                Address = created.Address,
                IsActive = created.IsActive
            };
        }
        #endregion

        #region Inventory
        public async Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto dto)
        {
            var inventory = new Inventory
            {
                ProductID = dto.ProductID,
                BatchNumber = dto.BatchNumber,
                ManufacturingDate = dto.ManufacturingDate,
                ExpirationDate = dto.ExpirationDate,
                QuantityOnHand = dto.QuantityOnHand,
                ReorderLevel = dto.ReorderLevel,
                CostPrice = dto.CostPrice,
                SellingPrice = dto.SellingPrice,
                IsActive = true
            };

            var created = await _postRepo.CreateInventoryAsync(inventory);

            return new InventoryDto
            {
                InventoryID = created.InventoryID,
                ProductID = created.ProductID,
                BatchNumber = created.BatchNumber,
                ManufacturingDate = created.ManufacturingDate,
                ExpirationDate = created.ExpirationDate,
                QuantityOnHand = created.QuantityOnHand,
                ReorderLevel = created.ReorderLevel,
                CostPrice = created.CostPrice,
                SellingPrice = created.SellingPrice,
                DateAdded = created.DateAdded,
                IsActive = created.IsActive,
                IsExpired = created.IsExpired,
                IsNearExpiry = created.IsNearExpiry,
                IsCriticalStock = created.IsCriticalStock
            };
        }
        #endregion

        #region VAT
        public async Task<VatDto> CreateVATAsync(CreateVATDto dto)
        {
            var vat = new VAT
            {
                VATName = dto.VATName,
                VATRate = dto.VATRate,
                IsActive = true
            };

            var created = await _postRepo.CreateVATAsync(vat);

            return new VatDto
            {
                VATID = created.VATID,
                VATName = created.VATName,
                VATRate = created.VATRate,
                IsActive = created.IsActive,
                DateCreated = created.DateCreated
            };
        }
        #endregion

        #region PurchaseOrder
        public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
        {
            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderNumber = dto.PurchaseOrderNumber,
                DeliveryDate = dto.DeliveryDate,
                SupplierID = dto.SupplierID,
                Notes = dto.Notes,
                Status = "Pending",
                PurchaseOrderItems = dto.PurchaseOrderItems.Select(poi => new PurchaseOrderItem
                {
                    ProductID = poi.ProductID,
                    QuantityOrdered = poi.QuantityOrdered,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.QuantityOrdered * poi.UnitCost
                }).ToList()
            };

            // Calculate total amount
            purchaseOrder.TotalAmount = purchaseOrder.PurchaseOrderItems.Sum(poi => poi.TotalCost);

            var created = await _postRepo.CreatePurchaseOrderAsync(purchaseOrder);

            return new PurchaseOrderDto
            {
                PurchaseOrderID = created.PurchaseOrderID,
                PurchaseOrderNumber = created.PurchaseOrderNumber,
                OrderDate = created.OrderDate,
                DeliveryDate = created.DeliveryDate,
                SupplierID = created.SupplierID,
                TotalAmount = created.TotalAmount,
                Status = created.Status,
                Notes = created.Notes,
                PurchaseOrderItems = created.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemID = poi.PurchaseOrderItemID,
                    ProductID = poi.ProductID,
                    QuantityOrdered = poi.QuantityOrdered,
                    QuantityReceived = poi.QuantityReceived,
                    UnitCost = poi.UnitCost,
                    TotalCost = poi.TotalCost
                }).ToList()
            };
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
                QuantityChanged = dto.QuantityChanged,
                QuantityBefore = inventory.QuantityOnHand,
                QuantityAfter = inventory.QuantityOnHand + dto.QuantityChanged,
                ReferenceNumber = dto.ReferenceNumber,
                Notes = dto.Notes,
                EmployeeID = dto.EmployeeID
            };

            var created = await _postRepo.CreateStockMovementAsync(stockMovement);

            // Update the inventory quantity
            inventory.QuantityOnHand = stockMovement.QuantityAfter;
            await _updateRepo.UpdateInventoryAsync(inventory);

            return new StockMovementDto
            {
                StockMovementID = created.StockMovementID,
                InventoryID = created.InventoryID,
                MovementType = created.MovementType,
                QuantityChanged = created.QuantityChanged,
                QuantityBefore = created.QuantityBefore,
                QuantityAfter = created.QuantityAfter,
                ReferenceNumber = created.ReferenceNumber,
                Notes = created.Notes,
                MovementDate = created.MovementDate,
                EmployeeID = created.EmployeeID
            };
        }
        #endregion

    }
}
