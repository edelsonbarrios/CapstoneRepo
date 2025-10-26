using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyBackend.Repository.POST
{
    public class PostRepository : IPostRepositoryContracts
    {
        private readonly PharmacyContext _context;
        private readonly IConfiguration _config;
        private readonly JwtHelper _jwtHelper;
        public PostRepository(PharmacyContext context,
            IConfiguration config,
            JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _config = config;
        }
        #region Account
        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var account = await _context.Accounts.Where(a => a.Username.Equals(request.Username) && a.IsActive == true)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync();
            if (account is null)
                return null;
            if (!account.EncryptedPassword.Equals(request.Password))
                return null;
            string token = _jwtHelper.TokenGeneration(account);
            return token;
        }

        public async Task<bool> CreateAccountAsync(Account request)
        {
            await _context.Accounts.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public string TokenGeneration(Account request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, request.Employee.Role.ToString()),
                new Claim("EmployeeId", request.Employee.Id.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:SecretKey")));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                expires: DateTime.UtcNow.AddHours(_config.GetValue<int>("Jwt:TokenValidity")),
                claims: claims,
                signingCredentials: signingCreds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);

        }
        #endregion

        #region Employee
        public async Task<int> CreateEmployeeAsync(Employee request)
        {
            await _context.Employees.AddAsync(request);
            await _context.SaveChangesAsync();
            return request.Id;
        }
        #endregion

        #region EmployeeHistory
        public async Task<bool> CreateEmployeeHistoryAsync(EmployeeHistory request)
        {
            await _context.EmployeeHistory.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Sales
        public async Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction salesTransaction)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validation
                if (string.IsNullOrEmpty(salesTransaction.InvoiceNumber))
                    throw new ArgumentException("Invoice number is required");

                if (salesTransaction.EmployeeID <= 0)
                    throw new ArgumentException("Valid Employee ID is required");

                // Check duplicate invoice number
                var existingTransaction = await _context.SalesTransaction
                    .FirstOrDefaultAsync(st => st.InvoiceNumber == salesTransaction.InvoiceNumber);

                if (existingTransaction != null)
                    throw new InvalidOperationException($"Invoice number {salesTransaction.InvoiceNumber} already exists");

                // Validate foreign keys
                var employee = await _context.Employees.FindAsync(salesTransaction.EmployeeID);
                if (employee == null)
                    throw new ArgumentException($"Employee with ID {salesTransaction.EmployeeID} not found");

                if (salesTransaction.DiscountId > 0)
                {
                    var discount = await _context.Discounts.FindAsync(salesTransaction.DiscountId);
                    if (discount == null)
                        throw new ArgumentException($"Discount with ID {salesTransaction.DiscountId} not found");
                }
                
                if (salesTransaction.VATId.HasValue)
                {
                    var vat = await _context.VAT.FindAsync(salesTransaction.VATId.Value);
                    if (vat == null)
                        throw new ArgumentException($"VAT with ID {salesTransaction.VATId} not found");
                }
                //Validation For Payment
                if (salesTransaction.AmountPaid <= 0)
                    throw new ArgumentException("Payment amount must be greater than zero to complete transaction");


                // Process items and update inventory
                foreach (var item in salesTransaction.Items)
                {
                    await ProcessSalesItemAsync(item, salesTransaction.InvoiceNumber, salesTransaction.EmployeeID);
                }

                // Calculate totals
                salesTransaction.TotalAmount = salesTransaction.Items.Sum(si => si.Subtotal);
                salesTransaction.NetAmount = salesTransaction.TotalAmount - salesTransaction.DiscountAmount + salesTransaction.VATAmount;

                _context.SalesTransaction.Add(salesTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _context.SalesTransaction
                    .Include(st => st.Discount)
                    .Include(st => st.Employee)
                    .Include(st => st.VAT)
                    .Include(st => st.Items)
                        .ThenInclude(si => si.Product)
                    .FirstAsync(st => st.Id == salesTransaction.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error creating sales transaction: {ex.Message}", ex);
            }
        }

        public async Task<SalesItem> CreateSalesItemAsync(SalesItem salesItem)
        {
            try
            {
                if (salesItem.SalesTransactionId <= 0)
                    throw new ArgumentException("Valid Sales Transaction ID is required");

                var transaction = await _context.SalesTransaction.FindAsync(salesItem.SalesTransactionId);
                if (transaction == null)
                    throw new ArgumentException($"Sales Transaction with ID {salesItem.SalesTransactionId} not found");

                await ProcessSalesItemAsync(salesItem, transaction.InvoiceNumber, transaction.EmployeeID);

                _context.SalesItem.Add(salesItem);
                await _context.SaveChangesAsync();

                return await _context.SalesItem
                    .Include(si => si.Product)
                    .Include(si => si.SalesTransaction)
                    .FirstAsync(si => si.Id == salesItem.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating sales item: {ex.Message}", ex);
            }
        }

        private async Task ProcessSalesItemAsync(SalesItem salesItem, string invoiceNumber, int employeeId)
        {
            // Validation
            if (salesItem.ProductId <= 0)
                throw new ArgumentException("Valid Product ID is required");

            if (salesItem.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");


            var product = await _context.Product.FindAsync(salesItem.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {salesItem.ProductId} not found");

            // Validate unit type
            var validUnitTypes = new[] { "Box", "Sheet", "Piece" };
            if (string.IsNullOrEmpty(salesItem.UnitType) ||
                !validUnitTypes.Contains(salesItem.UnitType, StringComparer.OrdinalIgnoreCase))
            {
                salesItem.UnitType = "Piece"; // Default to piece for backward compatibility
            }

            // Get available inventory using FIFO and FEFO
            var availableInventories = await _context.Inventory
                .Where(i => i.ProductID == salesItem.ProductId &&
                           i.IsActive &&
                           i.ExpirationDate > DateTime.UtcNow)
                .OrderBy(i => i.ExpirationDate) // First Expiry First Out
                .ThenBy(i => i.DateAdded) // Then First In First Out
                .ToListAsync();

            if (!availableInventories.Any())
                throw new InvalidOperationException($"No available inventory found for product {product.ProductName}");

            // Calculate pieces needed based on unit type
            var totalPiecesNeeded = CalculatePiecesNeeded(salesItem.UnitType, salesItem.Quantity, availableInventories.First());

            // Check total availability
            var totalAvailablePieces = availableInventories.Sum(i => i.TotalPiecesAvailable);
            if (totalAvailablePieces < totalPiecesNeeded)
                throw new InvalidOperationException($"Insufficient stock for {product.ProductName}. Available: {totalAvailablePieces} pieces, Required: {totalPiecesNeeded} pieces");

            // Set unit price if not provided
            if (salesItem.UnitPrice <= 0)
            {
                var sampleInventory = availableInventories.First();
                salesItem.UnitPrice = salesItem.UnitType.ToLower() switch
                {
                    "box" => sampleInventory.BoxPrice,
                    "sheet" => sampleInventory.SheetPrice,
                    "piece" => sampleInventory.PiecePrice,
                    _ => sampleInventory.PiecePrice
                };
            }

            // Calculate subtotal
            salesItem.Subtotal = salesItem.Quantity * salesItem.UnitPrice;

            // Deduct from inventories using FIFO/FEFO with multi-unit logic
            var remainingPieces = totalPiecesNeeded;
            foreach (var inventory in availableInventories)
            {
                if (remainingPieces <= 0) break;

                // Calculate how many pieces this inventory can provide
                var availablePieces = inventory.TotalPiecesAvailable;

                var piecesToDeduct = Math.Min(remainingPieces, availablePieces);

                // 🔹 Determine the equivalent unit quantity for deduction
                var sheetsPerBox = (inventory.SheetsPerBox.HasValue && inventory.SheetsPerBox.Value > 0)
                    ? inventory.SheetsPerBox.Value
                    : 1;

                var piecesPerSheet = (inventory.PiecesPerSheet.HasValue && inventory.PiecesPerSheet.Value > 0)
                    ? inventory.PiecesPerSheet.Value
                    : 1;

                var unitQuantityToDeduct = salesItem.UnitType.ToLower() switch
                {
                    "box" => piecesToDeduct / (sheetsPerBox * piecesPerSheet),
                    "sheet" => piecesToDeduct / piecesPerSheet,
                    "piece" => piecesToDeduct,
                    _ => piecesToDeduct
                };

                // Call the deduction method using the **unit quantity**, not pieces
                await DeductFromSingleInventory(inventory, unitQuantityToDeduct, invoiceNumber, employeeId, salesItem.UnitType);

                remainingPieces -= piecesToDeduct;
            }
        }

        private int CalculatePiecesNeeded(string unitType, int quantity, Inventory sampleInventory)
        {
            var sheetsPerBox = sampleInventory.SheetsPerBox ?? 0;
            var piecesPerSheet = sampleInventory.PiecesPerSheet ?? 0;

            var hasSheets = (sampleInventory.SheetsPerBox ?? 0) > 0;
            var hasPieces = (sampleInventory.PiecesPerSheet ?? 0) > 0;

            if(hasSheets && hasPieces)
            {
                return unitType.ToLower() switch
                {
                    "box" => quantity * sheetsPerBox * piecesPerSheet,
                    "sheet" => quantity * piecesPerSheet,
                    "piece" => quantity,
                    _ => quantity
                };
            }
            if(hasSheets && !hasPieces)
            {
                return unitType.ToLower() switch
                {
                    "box" => quantity * sheetsPerBox,
                    "sheet" => quantity,
                    "piece" => quantity,
                    _ => quantity
                };
            }
            if (!hasSheets && !hasPieces)
            {
                return quantity;
            }
            return quantity;
        }

        private async Task<int> DeductFromSingleInventory(
            Inventory inventory,
            int piecesNeeded,
            string referenceNumber,
            int? employeeId,
            string unitType = "piece"
            ) 
         {
            var beforeBox = inventory.BoxQuantity;
            var beforeSheet = inventory.SheetQuantity;
            var beforePiece = inventory.PieceQuantity;

            var remainingToDeduct = piecesNeeded;
            int sheetsPerBox = inventory.SheetsPerBox ?? 0;
            int piecesPerSheet = inventory.PiecesPerSheet ?? 0;
            int piecesPerBox = inventory.PiecePerBox ?? (sheetsPerBox > 0 && piecesPerSheet > 0 ? sheetsPerBox * piecesPerSheet : 0);

            switch (unitType.ToLower())
            {
                case "piece":
                    // start from pieces → then sheets → then boxes
                    remainingToDeduct = await DeductFromPiecesFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
                    break;

                case "sheet":
                    // start from sheets → then boxes
                    remainingToDeduct = await DeductFromSheetsFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
                    break;

                case "box":
                    // start directly from boxes
                    remainingToDeduct = await DeductFromBoxesFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
                    break;
            }

            _context.Inventory.Update(inventory);
            await _context.SaveChangesAsync();

            return piecesNeeded - remainingToDeduct;
        }


        private async Task<int> DeductFromPiecesFirst(Inventory inventory, int remainingToDeduct, int piecesPerSheet, int piecesPerBox)
        {
            // 1️⃣ Deduct from loose pieces
            if (remainingToDeduct > 0 && inventory.PieceQuantity > 0)
            {
                var piecesFromLoose = Math.Min(remainingToDeduct, inventory.PieceQuantity);
                inventory.PieceQuantity -= piecesFromLoose;
                remainingToDeduct -= piecesFromLoose;
            }

            // 2️⃣ If no more loose pieces, break down sheets
            if (remainingToDeduct > 0 && inventory.SheetQuantity > 0 && piecesPerSheet > 0)
            {
                var sheetsToBreak = Math.Min((int)Math.Ceiling((double)remainingToDeduct / piecesPerSheet), inventory.SheetQuantity);
                inventory.SheetQuantity -= sheetsToBreak;
                inventory.PieceQuantity += sheetsToBreak * piecesPerSheet;

                // Try again deducting pieces now that we have more
                return await DeductFromPiecesFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
            }

            // 3️⃣ If still not enough, break down boxes
            if (remainingToDeduct > 0 && inventory.BoxQuantity > 0 && piecesPerBox > 0)
            {
                var boxesToBreak = Math.Min((int)Math.Ceiling((double)remainingToDeduct / piecesPerBox), inventory.BoxQuantity);
                inventory.BoxQuantity -= boxesToBreak;
                inventory.SheetQuantity += boxesToBreak * (inventory.SheetsPerBox ?? 0);
                inventory.PieceQuantity += boxesToBreak * piecesPerBox;

                return await DeductFromPiecesFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
            }

            return remainingToDeduct;
        }

        private async Task<int> DeductFromSheetsFirst(Inventory inventory, int remainingToDeduct, int piecesPerSheet, int piecesPerBox)
        {
            // 1️⃣ Deduct from sheets first
            if (remainingToDeduct > 0 && inventory.SheetQuantity > 0)
            {
                var sheetsToUse = Math.Min(remainingToDeduct, inventory.SheetQuantity);
                inventory.SheetQuantity -= sheetsToUse;
                remainingToDeduct -= sheetsToUse;
            }

            // 2️⃣ If not enough sheets, break boxes
            if (remainingToDeduct > 0 && inventory.BoxQuantity > 0 && (inventory.SheetsPerBox ?? 0) > 0)
            {
                var boxesToBreak = Math.Min((int)Math.Ceiling((double)remainingToDeduct / inventory.SheetsPerBox.Value), inventory.BoxQuantity);
                inventory.BoxQuantity -= boxesToBreak;
                inventory.SheetQuantity += boxesToBreak * inventory.SheetsPerBox.Value;

                return await DeductFromSheetsFirst(inventory, remainingToDeduct, piecesPerSheet, piecesPerBox);
            }

            return remainingToDeduct;
        }

        private async Task<int> DeductFromBoxesFirst(Inventory inventory, int remainingToDeduct, int piecesPerSheet, int piecesPerBox)
        {
            if (remainingToDeduct > 0 && inventory.BoxQuantity > 0)
            {
                var boxesToUse = Math.Min(remainingToDeduct, inventory.BoxQuantity);
                inventory.BoxQuantity -= boxesToUse;
                remainingToDeduct -= boxesToUse;
            }
            return remainingToDeduct;
        }




        #endregion

        #region Discount
        public async Task<bool> CreateDiscountAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ValidateDiscountNameAsync(string name)
        {
            var validation = await _context.Discounts.AnyAsync(d => d.DiscountName.Equals(name) && d.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Product
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
        #endregion

        #region Category
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> ValidateCategoryNameAsync(string name)
        {
            var validation = await _context.Category.AnyAsync(c => c.CategoryName.Equals(name) && c.IsActive);
            if(validation) 
                return false;
            return true;
        }
        #endregion

        #region Brand
        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            await _context.Brand.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> ValidateBrandNameAsync(string name)
        {
            var validation = await _context.Brand.AnyAsync(b => b.BrandName.Equals(name) && b.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Generic
        public async Task<Generic> CreateGenericAsync(Generic generic)
        {
            await _context.Generic.AddAsync(generic);
            await _context.SaveChangesAsync();
            return generic;
        }
        public async Task<bool> ValidateGenericNameAsync(string name)
        {
            var validation = await _context.Generic.AnyAsync(g => g.GenericName.Equals(name) && g.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Formulation
        public async Task<Formulation> CreateFormulationAsync(Formulation formulation)
        {
            await _context.Formulation.AddAsync(formulation);
            await _context.SaveChangesAsync();
            return formulation;
        }
        public async Task<bool> ValidateFormulationNameAsync(string name)
        {
            var validation = await _context.Formulation.AnyAsync(f => f.FormulationName.Equals(name) && f.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Classification
        public async Task<Classification> CreateClassificationAsync(Classification classification)
        {
            await _context.Classification.AddAsync(classification);
            await _context.SaveChangesAsync();
            return classification;
        }
        public async Task<bool> ValidateClassificationNameAsync(string name)
        {
            var validation = await _context.Classification.AnyAsync(c => c.ClassificationName.Equals(name) && c.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Type
        public async Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel)
        {
            await _context.TypeModel.AddAsync(typeModel);
            await _context.SaveChangesAsync();
            return typeModel;
        }
        public async Task<bool> ValidateTypeNameAsync(string name)
        {
            var validation = await _context.TypeModel.AnyAsync(t => t.TypeName.Equals(name) && t.IsActive);
            if (validation)
                return false;
            return true;
        }
        #endregion

        #region Supplier
        public async Task<bool> CreateSupplierAsync(Supplier supplier)
        {
            supplier.Products = new List<Product>();

            await _context.Supplier.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Inventory
        public async Task<bool> CreateInventoryAsync(Inventory inventory)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create inventory (your existing logic)
                await _context.Inventory.AddAsync(inventory);
                await _context.SaveChangesAsync();

                // AUTO-CREATE stock movement for stock in
                var stockMovement = new StockMovement
                {
                    InventoryID = inventory.InventoryID,
                    MovementType = "Stock In",
                    BoxQuantityChanged = inventory.BoxQuantity,
                    SheetQuantityChanged = inventory.SheetQuantity,
                    PieceQuantityChanged = inventory.PieceQuantity,
                    BoxQuantityBefore = 0,
                    SheetQuantityBefore = 0,
                    PieceQuantityBefore = 0,
                    BoxQuantityAfter = inventory.BoxQuantity,
                    SheetQuantityAfter = inventory.SheetQuantity,
                    PieceQuantityAfter = inventory.PieceQuantity,
                    ReferenceNumber = $"SI-{DateTime.Now:yyyyMMdd}-{inventory.InventoryID}",
                    Notes = $"Initial stock entry for batch {inventory.BatchNumber}",
                    MovementDate = DateTime.UtcNow
                };

                await _context.StockMovement.AddAsync(stockMovement);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error creating inventory with stock movement: {ex.Message}", ex);
            }
        }
        #endregion

        #region VAT
        public async Task<bool> CreateVATAsync(VAT vat)
        {
            await _context.VAT.AddAsync(vat);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> VatNameValidationAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var vatNameValidation = await _context.VAT.AnyAsync(v => v.VATName.Equals(name) && v.IsActive == true);
            if (vatNameValidation)
                return false;
            return true;
        }
        #endregion

        #region PurchaseOrder
        public async Task<bool> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrder.AddAsync(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string> GenerateNewPurchaseOrderNumberAsync()
        {
            int currentYear = DateTime.Now.Year;
            int newNumber = 1; // Default starting number if none found

            // Find the latest PO for the current year
            var lastPo = await _context.PurchaseOrder
                .Where(po => po.PurchaseOrderNumber.StartsWith($"PO-{currentYear}-"))
                .OrderByDescending(po => po.PurchaseOrderNumber)
                .Select(po => po.PurchaseOrderNumber)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(lastPo))
            {
                // Extract numeric part from the last PO number
                var match = System.Text.RegularExpressions.Regex.Match(lastPo, @"(\d+)$");
                if (match.Success && int.TryParse(match.Value, out int lastNumber))
                {
                    newNumber = lastNumber + 1;
                }
            }

            // Format with leading zeros, e.g. PO-2025-0001
            string newPurchaseOrderNumber = $"PO-{currentYear}-{newNumber:D4}";
            return newPurchaseOrderNumber;
        }

        #endregion

        #region PurchaseOrderItem
        public async Task<bool> CreatePurchaseOrderItemsAsync(List<PurchaseOrderItem> request)
        {
            await _context.PurchaseOrderItem.AddRangeAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region StockMovement
        public async Task<StockMovement> CreateStockMovementAsync(StockMovement stockMovement)
        {
            await _context.StockMovement.AddAsync(stockMovement);
            await _context.SaveChangesAsync();
            return stockMovement;
        }
        #endregion
    }
}
