using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models.CashierViewModel;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;

namespace PharmacyBackend.Service.PostService
{
    public class PostService : IPostServiceContracts
    {
        private readonly IPostRepositoryContracts _postRepo;
        private readonly IGetRepositoryContracts _getRepo;
        private readonly IUpdateRepositoryContracts _updateRepo;

        public PostService(IPostRepositoryContracts postRepo,
            IGetRepositoryContracts getRepo,
            IUpdateRepositoryContracts updateRepo)
        {
            _postRepo = postRepo;
            _getRepo = getRepo;         
            _updateRepo = updateRepo;
        }

        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var enteredPassword = AesHelper.Encrypt(request.Password);
            request.Password = enteredPassword;
            var token = await _postRepo.AuthenticationAsync(request);
            return token;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountRequest request)
        {
            if (request is null)
                return false;

            var encryptedPass = AesHelper.Encrypt(request.Password);
            var newAcc = new Account()
            {
                AccountCode = request.AccountCode,
                Username = request.Username,
                EncryptedPassword = encryptedPass,
                EmployeeId = request.EmployeeId
            };

            var createAccount = await _postRepo.CreateAccountAsync(newAcc);
            if (!createAccount)
                return false;
            return createAccount;
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (request is null)
                return 0;

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
                Address = request.Address,
                RoleId = roleId
            };

            var employeeId = await _postRepo.CreateEmployeeAsync(newEmployee);
            if (employeeId == 0)
                return 0;
            return employeeId;
        }

        //sale 

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

        //Discount
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
        //Product
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

        //Category
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

        //ItemDataControl

        //Brand
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

        //Generic

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

        //Formulation
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

        //Classification
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

        //Type
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
        //Supplier
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

        //Inventory
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

        //VAT
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

        //PurchaseOrder
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

        //StockMovement
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
    }
}
