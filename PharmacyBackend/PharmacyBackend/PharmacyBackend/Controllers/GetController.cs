using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.Models;
using System.Diagnostics.Contracts;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.Service.GetService;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private readonly IGetServiceContracts _getService;
        private readonly ILogger<GetController> _logger;
        private readonly IConfiguration _config;

        public GetController(IGetServiceContracts getService, ILogger<GetController> logger, IConfiguration config)
        {
            _getService = getService;
            _logger = logger;
            _config = config;
        }

        #region DashBoardInventoryMan

        [HttpGet("InventoryDashboard")]
        public async Task<IActionResult> GetInventoryDashboard()
        {
            try
            {
                var dashboard = await _getService.GetInventoryDashboardAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving inventory dashboard", error = ex.Message });
            }
        }

        #endregion

        #region Employee
        [Authorize(Roles = "Administrator")]
        [HttpGet("EmployeeAll")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployeesAsync()
        {
            var employees = await _getService.GetAllEmployeesAsync();

            if (!employees.Any() || employees.Count() == 0)
                return NotFound("There is no employees currently registered.");
            return Ok(employees);
        }
        [HttpGet("Employee")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _getService.GetEmployeeByIdAsync(id);
            if (employee is null)
                return NotFound("Employee does not exist.");
            return Ok(employee);
        }
        [HttpGet("EmployeeLastName")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployeeByLastNameAsync(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                return BadRequest("Last name field is empty. Please try again.");
            var employees = await _getService.GetEmployeeByLastName(lastName);
            if (employees is null)
                return NotFound("Employee does not exist.");
            return Ok(employees);
        }

        [HttpGet("DisplayEmployees")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetDisplayEmployeesAsync()
        {
            var employees = await _getService.GetFirstDisplayEmployees();
            if (!employees.Any() || employees.Count() == 0)
                return NotFound("There is no employees currently registered.");
            return Ok(employees);
        }
        #endregion

        #region EmployeeHistory
        [HttpGet("EmployeeHistoryAll")]
        public async Task<ActionResult<List<EmployeeHistoryDTO>>> GetAllEmployeeHistoryAsync()
        {
            var employeeHistories = await _getService.GetAllEmployeeHistoryAsync();
            if (!employeeHistories.Any() || employeeHistories.Count() == 0)
                return NotFound("No actions currently performed.");
            return Ok(employeeHistories);
        }
        [HttpGet("EmployeeHistoryDisplay")]
        public async Task<ActionResult<List<EmployeeHistoryDTO>>> GetDisplayEmployeeHistoriesAsync()
        {
            var employeeHistories = await _getService.GetDisplayEmployeeHistoryAsync();
            if (!employeeHistories.Any() || employeeHistories.Count() == 0)
                return NotFound("Employee History is empty.");
            return Ok(employeeHistories);
        }
        #endregion

        #region Account
        [HttpGet("AccountAll")]
        public async Task<ActionResult<List<AccountDTO>>> GetAllAccountsAsync()
        {
            var accounts = await _getService.GetAllAccountsAsync();
            if (!accounts.Any() || accounts.Count() == 0)
                return NotFound("There is no account currently registered.");
            return Ok(accounts);
        }
        [HttpGet("Account")]
        public async Task<ActionResult<AccountDTO>> GetAccountByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest("Account does not exist.");
            var account = await _getService.GetAccountByIdAsync(id);
            if (account is null)
                return NotFound("Account does not exist.");
            return Ok(account);
        }
        [HttpGet("SearchAccount")]
        public async Task<ActionResult<AccountDTO>> SearchAccountByUsernameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("Username field is empty.");
            var account = await _getService.SearchAccountByUsernameAsync(userName);
            if (account is null)
                return NotFound("Username does not exist.");
            return Ok(account);
        }
        [HttpGet("AccountEmployeeId")] 
        public async Task<ActionResult<AccountDTO>> GetAccountByEmployeeIdAsync(int employeeId)
        {
            if (employeeId == 0)
                return NotFound(new AccountDTO());
            var account = await _getService.GetAccountByEmployeeIdAsync(employeeId);
            if (account is null)
                return BadRequest(new AccountDTO());
            return account;
        }
        #endregion

        #region Daily Sales Endpoints

        [HttpGet("DailySalesSummary")]
        public async Task<ActionResult<DailySalesSummaryDto>> GetSummarySalesForToday()
        {
            var summary = await _getService.GetSummarySalesForToday();
            if (summary is null)
                return NotFound("No transactions for today.");
            return Ok(summary);
        }
        [HttpGet("daily-sales/summary")]
        public async Task<IActionResult> GetDailySalesSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? employeeId = null,
            [FromQuery] string paymentMethod = null)
        {
            try
            {
                // Validate dates
                if (startDate > endDate)
                {
                    return BadRequest(new { message = "Start date cannot be after end date" });
                }

                var summary = await _getService.GetDailySalesSummaryAsync(
                    startDate,
                    endDate,
                    employeeId,
                    paymentMethod
                );

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving daily sales summary", error = ex.Message });
            }
        }

        [HttpGet("daily-sales/products")]
        public async Task<IActionResult> GetProductSalesSummary(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                // Validate dates
                if (startDate > endDate)
                {
                    return BadRequest(new { message = "Start date cannot be after end date" });
                }

                var products = await _getService.GetProductSalesSummaryAsync(startDate, endDate);

                if (!products.Any())
                {
                    return Ok(new List<ProductSalesSummaryDto>());
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving product sales summary", error = ex.Message });
            }
        }

        [HttpGet("daily-sales/transactions")]
        public async Task<IActionResult> GetFilteredSalesTransactions(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? employeeId = null,
            [FromQuery] string paymentMethod = null)
        {
            try
            {
                // Validate dates
                if (startDate > endDate)
                {
                    return BadRequest(new { message = "Start date cannot be after end date" });
                }

                var transactions = await _getService.GetFilteredSalesTransactionsAsync(
                    startDate,
                    endDate,
                    employeeId,
                    paymentMethod
                );

                if (!transactions.Any())
                {
                    return Ok(new List<SalesTransactionDto>());
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving sales transactions", error = ex.Message });
            }
        }

        #endregion

        #region Sales
        [HttpGet("TransactionId")]
        public async Task<IActionResult> GetSalesTransactionByIdAsync(int id)
        {
            var transaction = await _getService.GetSalesTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound($"Sales transaction with ID {id} not found.");

            return Ok(transaction);
        }

        [HttpGet("TransactionAll")]
        public async Task<IActionResult> GetAllSalesTransactionsAsync()
        {
            var transactions = await _getService.GetAllSalesTransactionsAsync();
            if (!transactions.Any())
                return NotFound("No sales transactions found.");

            return Ok(transactions);
        }

        [HttpGet("TransactionByEmployeeId")]
        public async Task<IActionResult> GetSalesTransactionsByEmployeeIdAsync(int employeeId)
        {
            var transactions = await _getService.GetSalesTransactionsByEmployeeIdAsync(employeeId);
            if (!transactions.Any())
                return NotFound("No sales transactions found for this employee.");

            return Ok(transactions);
        }

        [HttpGet("TransactionByDateRange")]
        public async Task<IActionResult> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _getService.GetSalesTransactionsByDateRangeAsync(startDate, endDate);
            if (!transactions.Any())
                return NotFound("No sales transactions found in the given date range.");

            return Ok(transactions);
        }

        [HttpGet("TransactionByInvoice")]
        public async Task<IActionResult> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber)
        {
            var transactions = await _getService.GetSalesTransactionsByInvoiceNumberAsync(invoiceNumber);
            if (!transactions.Any())
                return NotFound($"No sales transactions found for invoice number {invoiceNumber}.");

            return Ok(transactions);
        }

        [HttpGet("Item")]
        public async Task<IActionResult> GetSalesItemByIdAsync(int id)
        {
            var item = await _getService.GetSalesItemByIdAsync(id);
            if (item == null)
                return NotFound($"Sales item with ID {id} not found.");

            return Ok(item);
        }

        [HttpGet("ItemsByTransaction")]
        public async Task<IActionResult> GetSalesItemsByTransactionIdAsync(int transactionId)
        {
            var items = await _getService.GetSalesItemsByTransactionIdAsync(transactionId);
            if (!items.Any())
                return NotFound("No sales items found for this transaction.");

            return Ok(items);
        }

        [HttpGet("RefNumValidation")]
        public async Task<ActionResult<object>> ReferenceNumberValidationAsync(string refNum) 
        {
          if(string.IsNullOrEmpty(refNum))
            return BadRequest(new {Message = "Reference number cannot be empty."});
          var referenceNumberValidation = await _getService.ReferenceNumberValidationAsync(refNum);
          if(!referenceNumberValidation)
            return BadRequest(new {Message = "Reference number is already used."});
          return Ok(new {Message = "Reference number is good."});
        }


        #endregion

        #region Discount 
        [HttpGet("DiscountAll")]
        public async Task<IActionResult> GetAllDiscountsAsync()
        {
            var discounts = await _getService.GetAllDiscountAsync();
            if (discounts == null || !discounts.Any())
                return NotFound(new { Message = "No discounts found." });

            return Ok(discounts);
        }

        [HttpGet("Discount")]
        public async Task<IActionResult> GetDiscountByIdAsync(int id)
        {
            var discount = await _getService.GetDiscountByIdAsync(id);
            if (discount == null)
                return NotFound(new { Message = "Discount not found." });

            return Ok(discount);
        }
        #endregion

        #region Product
        [HttpGet("ProductAll")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var products = await _getService.GetAllProductAsync();
            if (products == null || !products.Any())
                return NotFound(new { Message = "No products found." });

            return Ok(products);
        }

        [HttpGet("Product")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _getService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            return Ok(product);
        }
        [HttpGet("InactiveProducts")]
        public async Task<ActionResult<List<ProductDto>>> GetAllInactiveProductsAsync() 
        {
          var inactiveProducts = await _getService.GetAllInactiveProductsAsync();
          if(!inactiveProducts.Any() || inactiveProducts.Count() == 0)
            return NotFound(new List<ProductDto>());
          return Ok(inactiveProducts);
        }

        [HttpGet("ProductName")]
        public async Task<ActionResult<ProductDto>> GetProductByNameAsync(string name) 
        {
          if(string.IsNullOrEmpty(name))
            return NotFound(new ProductDto());

          var product = await _getService.GetProductByNameAsync(name);
          if(product is null)
            return NotFound(new ProductDto());

          return Ok(product);
        }

        #endregion

        #region Category
        [HttpGet("CategoryAll")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var categories = await _getService.GetAllCategoryAsync();
            if (categories == null || !categories.Any())
                return NotFound(new { Message = "No categories found." });

            return Ok(categories);
        }

        [HttpGet("Category")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _getService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = "Category not found." });

            return Ok(category);
        }

        [HttpGet("InactiveCategories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllInactiveCategoriesAsync() 
        {
          var inactiveCategories = await _getService.GetAllInactiveCategoriesAsync();
          if(!inactiveCategories.Any() || inactiveCategories.Count() == 0)
            return NotFound(new List<CategoryDto>());
          return Ok(inactiveCategories);
        }
        #endregion

        #region Brand
        [HttpGet("BrandAll")]
        public async Task<IActionResult> GetAllBrandAsync()
        {
            var brands = await _getService.GetAllBrandAsync();
            if (brands == null || !brands.Any())
                return NotFound(new { Message = "No brands found." });

            return Ok(brands);
        }

        [HttpGet("Brand")]
        public async Task<IActionResult> GetBrandByIdAsync(int id)
        {
            var brand = await _getService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound(new { Message = "Brand not found." });

            return Ok(brand);
        }
        #endregion

        #region Generic
        [HttpGet("GenericAll")]
        public async Task<IActionResult> GetAllGenericAsync()
        {
            var generics = await _getService.GetAllGenericAsync();
            if (generics == null || !generics.Any())
                return NotFound(new { Message = "No generics found." });

            return Ok(generics);
        }

        [HttpGet("Generic")]
        public async Task<IActionResult> GetGenericByIdAsync(int id)
        {
            var generic = await _getService.GetGenericByIdAsync(id);
            if (generic == null)
                return NotFound(new { Message = "Generic not found." });

            return Ok(generic);
        }
        #endregion

        #region Formulation
        [HttpGet("FormulationAll")]
        public async Task<IActionResult> GetAllFormulationAsync()
        {
            var formulations = await _getService.GetAllFormulationAsync();
            if (formulations == null || !formulations.Any())
                return NotFound(new { Message = "No formulations found." });

            return Ok(formulations);
        }

        [HttpGet("Formulation")]
        public async Task<IActionResult> GetFormulationByIdAsync(int id)
        {
            var formulation = await _getService.GetFormulationByIdAsync(id);
            if (formulation == null)
                return NotFound(new { Message = "Formulation not found." });

            return Ok(formulation);
        }
        #endregion

        #region Classification
        [HttpGet("ClassificationAll")]
        public async Task<IActionResult> GetAllClassificationAsync()
        {
            var classifications = await _getService.GetAllClassificationAsync();
            if (classifications == null || !classifications.Any())
                return NotFound(new { Message = "No classifications found." });

            return Ok(classifications);
        }

        [HttpGet("Classification")]
        public async Task<IActionResult> GetClassificationByIdAsync(int id)
        {
            var classification = await _getService.GetClassificationByIdAsync(id);
            if (classification == null)
                return NotFound(new { Message = "Classification not found." });

            return Ok(classification);
        }
        #endregion

        #region Type
        [HttpGet("TypeAll")]
        public async Task<IActionResult> GetAllTypeModelAsync()
        {
            var types = await _getService.GetAllTypeModelAsync();
            if (types == null || !types.Any())
                return NotFound(new { Message = "No types found." });

            return Ok(types);
        }

        [HttpGet("Type")]
        public async Task<IActionResult> GetTypeModelByIdAsync(int id)
        {
            var type = await _getService.GetTypeModelByIdAsync(id);
            if (type == null)
                return NotFound(new { Message = "Type not found." });

            return Ok(type);
        }
        #endregion

        #region Supplier
        [HttpGet("SupplierAll")]
        public async Task<ActionResult<List<SupplierDTO>>> GetAllSupplierAsync()
        {
            var suppliers = await _getService.GetAllSupplierAsync();
            if (!suppliers.Any() || suppliers.Count() == 0)
                return NotFound("No suppliers found.");
            return Ok(suppliers);
        }

        [HttpGet("SupplierId")]
        public async Task<ActionResult<SupplierDTO>> GetSupplierByIdAsync(int id)
        {
            var supplier = await _getService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound("Supplier not found.");
            return Ok(supplier);
        }
        #endregion

        #region Inventory
        [HttpGet("InventoryAll")]
        public async Task<IActionResult> GetAllInventoryAsync()
        {
            var inventories = await _getService.GetAllInventoryAsync();
            if (!inventories.Any())
                return NotFound("No inventory found.");
            return Ok(inventories);
        }
        [HttpGet("InventoryAvailable")]
        public async Task<ActionResult<List<InventoryDto>>> GetAllAvailableInventoryAsync()
        {
            var inventories = await _getService.GetAllAvailableInventoryAsync();
            if (inventories.Count() == 0)
                return NotFound("No products registered in the inventory.");
            return Ok(inventories);
        }

        [HttpGet("InventoryId")]
        public async Task<IActionResult> GetInventoryByIdAsync(int id)
        {
            var inventory = await _getService.GetInventoryByIdAsync(id);
            if (inventory == null)
                return NotFound("Inventory not found.");
            return Ok(inventory);
        }

        [HttpGet("InventoryByProduct")]
        public async Task<IActionResult> GetInventoryByProductIdAsync(int productId)
        {
            var inventories = await _getService.GetInventoryByProductIdAsync(productId);
            if (!inventories.Any())
                return NotFound("No inventory found for this product.");
            return Ok(inventories);
        }

        [HttpGet("InventoryExpired")]
        public async Task<IActionResult> GetExpiredInventoryAsync()
        {
            var inventories = await _getService.GetExpiredInventoryAsync();
            if (!inventories.Any())
                return NotFound("No expired inventory found.");
            return Ok(inventories);
        }

        [HttpGet("InventoryNearExpiry")]
        public async Task<IActionResult> GetNearExpiryInventoryAsync()
        {
            var inventories = await _getService.GetNearExpiryInventoryAsync();
            if (!inventories.Any())
                return NotFound("No near expiry inventory found.");
            return Ok(inventories);
        }

        [HttpGet("InventoryCriticalStock")]
        public async Task<IActionResult> GetCriticalStockInventoryAsync()
        {
            var inventories = await _getService.GetCriticalStockInventoryAsync();
            if (!inventories.Any())
                return NotFound("No critical stock inventory found.");
            return Ok(inventories);
        }
        #endregion

        #region VAT
        [HttpGet("VATAll")]
        public async Task<IActionResult> GetAllVATAsync()
        {
            var vats = await _getService.GetAllVATAsync();
            if (!vats.Any())
                return NotFound("No VAT entries found.");
            return Ok(vats);
        }

        [HttpGet("VATId")]
        public async Task<IActionResult> GetVATByIdAsync(int id)
        {
            var vat = await _getService.GetVATByIdAsync(id);
            if (vat == null)
                return NotFound("VAT not found.");
            return Ok(vat);
        }
        #endregion

        #region PurchaseOrder
        [HttpGet("PurchaseOrderAll")]
        public async Task<IActionResult> GetAllPurchaseOrderAsync()
        {
            var orders = await _getService.GetAllPurchaseOrderAsync();
            if (!orders.Any())
                return NotFound("No purchase orders found.");
            return Ok(orders);
        }

        [HttpGet("PurchaseOrderId")]
        public async Task<IActionResult> GetPurchaseOrderByIdAsync(int id)
        {
            var order = await _getService.GetPurchaseOrderByIdAsync(id);
            if (order == null)
                return NotFound("Purchase order not found.");
            return Ok(order);
        }

        [HttpGet("PurchaseOrderBySupplier")]
        public async Task<IActionResult> GetPurchaseOrderBySupplierIdAsync(int supplierId)
        {
            var orders = await _getService.GetPurchaseOrderBySupplierIdAsync(supplierId);
            if (!orders.Any())
                return NotFound("No purchase orders found for this supplier.");
            return Ok(orders);
        }

        [HttpGet("PurchaseOrderByStatus")]
        public async Task<IActionResult> GetPurchaseOrderByStatusAsync(string status)
        {
            var orders = await _getService.GetPurchaseOrderByStatusAsync(status);
            if (!orders.Any())
                return NotFound($"No purchase orders found with status: {status}");
            return Ok(orders);
        }
        [HttpGet("PurchaseOrderPendings")]
        public async Task<ActionResult<List<PurchaseOrderDTO>>> GetPendingPurchaseOrdersAsync()
        {
            var purchaseOrders = await _getService.GetPendingPurchaseOrdersAsync();
            if (!purchaseOrders.Any() || purchaseOrders.Count() == 0)
                return NotFound("No Purchase Orders currently created.");
            return Ok(purchaseOrders);
        }
        #endregion

        #region StockMovement
        [HttpGet("StockMovementAll")]
        public async Task<IActionResult> GetAllStockMovementAsync()
        {
            var movements = await _getService.GetAllStockMovementAsync();
            if (!movements.Any())
                return NotFound("No stock movements found.");
            return Ok(movements);
        }

        [HttpGet("StockMovementId")]
        public async Task<IActionResult> GetStockMovementByIdAsync(int id)
        {
            var movement = await _getService.GetStockMovementByIdAsync(id);
            if (movement == null)
                return NotFound("Stock movement not found.");
            return Ok(movement);
        }

        [HttpGet("StockMovementByInventory")]
        public async Task<IActionResult> GetStockMovementByInventoryIdAsync(int inventoryId)
        {
            var movements = await _getService.GetStockMovementByInventoryIdAsync(inventoryId);
            if (!movements.Any())
                return NotFound("No stock movements found for this inventory.");
            return Ok(movements);
        }

        [HttpGet("StockMovementByProduct")]
        public async Task<IActionResult> GetStockMovementByProductIdAsync(int productId)
        {
            var movements = await _getService.GetStockMovementByProductIdAsync(productId);
            if (!movements.Any())
                return NotFound("No stock movements found for this product.");
            return Ok(movements);
        }
        #endregion

        #region Reports

        //Sales - Reports

        [HttpGet("reports/sales")]
        public async Task<IActionResult> GetSalesReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? employeeId = null,
            [FromQuery] string paymentMethod = null)
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
                    : null;
                DateTime? endDateUtc = endDate.HasValue
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
                    : null;

                var report = await _getService.GetSalesReportAsync(
                    startDateUtc,
                    endDateUtc,
                    employeeId,
                    paymentMethod);

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error generating sales report: {ex.Message}" });
            }
        }
        
        [HttpGet("reports/sales/export")]
        public async Task<IActionResult> ExportSalesReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? employeeId = null,
            [FromQuery] string paymentMethod = null)
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue 
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc) 
                    : null;
                DateTime? endDateUtc = endDate.HasValue 
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc) 
                    : null;

                var excelData = await _getService.ExportSalesReportToExcelAsync(
                    startDateUtc,
                    endDateUtc,
                    employeeId,
                    paymentMethod);

                var fileName = startDate.HasValue && endDate.HasValue
                    ? $"SalesReport_{startDate.Value:yyyyMMdd}_{endDate.Value:yyyyMMdd}.xlsx"
                    : $"SalesReport_AllTime_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error exporting sales report: {ex.Message}" });
            }
        }
        

        //Inventory - Reports

        [HttpGet("reports/inventory")]
        public async Task<IActionResult> GetInventoryReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string filterType = "all")
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
                    : null;
                DateTime? endDateUtc = endDate.HasValue
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
                    : null;

                var report = await _getService.GetInventoryReportAsync(
                    startDateUtc,
                    endDateUtc,
                    filterType);

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error generating inventory report: {ex.Message}" });
            }
        }

        
        [HttpGet("reports/inventory/export")]
        public async Task<IActionResult> ExportInventoryReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string filterType = "all")
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue 
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc) 
                    : null;
                DateTime? endDateUtc = endDate.HasValue 
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc) 
                    : null;

                var excelData = await _getService.ExportInventoryReportToExcelAsync(
                    startDateUtc,
                    endDateUtc,
                    filterType);

                var fileName = startDate.HasValue && endDate.HasValue
                    ? $"InventoryReport_{startDate.Value:yyyyMMdd}_{endDate.Value:yyyyMMdd}_{filterType}.xlsx"
                    : $"InventoryReport_AllTime_{filterType}_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error exporting inventory report: {ex.Message}" });
            }
        }
        

        //Fast-Moving Products

        [HttpGet("reports/fast-moving-products")]
        public async Task<IActionResult> GetFastMovingProductsReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int topN = 20)
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
                    : null;
                DateTime? endDateUtc = endDate.HasValue
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
                    : null;

                var report = await _getService.GetFastMovingProductsReportAsync(
                    startDateUtc,
                    endDateUtc,
                    topN);

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error generating fast-moving products report: {ex.Message}" });
            }
        }

        
        [HttpGet("reports/fast-moving-products/export")]
        public async Task<IActionResult> ExportFastMovingProductsReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int topN = 20)
        {
            try
            {
                // Convert to UTC only if dates are provided
                DateTime? startDateUtc = startDate.HasValue 
                    ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc) 
                    : null;
                DateTime? endDateUtc = endDate.HasValue 
                    ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc) 
                    : null;

                var excelData = await _getService.ExportFastMovingProductsToExcelAsync(
                    startDateUtc,
                    endDateUtc,
                    topN);

                var fileName = startDate.HasValue && endDate.HasValue
                    ? $"FastMovingProducts_{startDate.Value:yyyyMMdd}_{endDate.Value:yyyyMMdd}_Top{topN}.xlsx"
                    : $"FastMovingProducts_AllTime_Top{topN}_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error exporting fast-moving products report: {ex.Message}" });
            }
        }

        #endregion

        #region BackUp Database

        /// <summary>
        /// Get all available backups
        /// </summary>
        [HttpGet("backups/list")]
        //[Authorize] // Add role if needed: [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BackupListResponse>> GetAllBackups()
        {
            try
            {
                var response = await _getService.GetAllBackupsAsync();

                if (response.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBackups endpoint");
                return StatusCode(500, new BackupListResponse
                {
                    Success = false,
                    Backups = new List<BackupDto>(),
                    TotalCount = 0,
                    TotalSizeMB = "0.00 MB"
                });
            }
        }
        /// <summary>
        /// Get current backup configuration settings
        /// </summary>
        [HttpGet("backups/settings")]
        public async Task<ActionResult<BackupSettingsDto>> GetBackupSettings()
        {
            var settings = await _getService.GetBackupSettingsAsync();
            return Ok(settings);
        }
        /// <summary>
        /// Download a specific backup file
        /// </summary>
        [HttpGet("backups/download/{fileName}")]
        //[Authorize] // Add role if needed: [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadBackup(string fileName)
        {
            try
            {
                // Validate filename to prevent directory traversal attacks
                if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                {
                    return BadRequest(new { Success = false, Message = "Invalid filename" });
                }

                var fileBytes = await _getService.DownloadBackupAsync(fileName);

                if (fileBytes == null)
                {
                    return NotFound(new { Success = false, Message = "Backup file not found" });
                }

                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DownloadBackup endpoint");
                return StatusCode(500, new { Success = false, Message = "Error downloading backup" });
            }
        }


        #endregion

        #region AdminDashboard 
        [HttpGet("TodaySalesAmount")]
        public async Task<ActionResult<decimal>> GetTodaySalesAmountAsync()
        {
            var todaySalesAmount = await _getService.GetTodaySalesAmountAsync();
            if (todaySalesAmount <= 0)
                return NotFound(0);
            return Ok(todaySalesAmount);
        }
        [HttpGet("InventoryLowStock")]
        public async Task<ActionResult<int>> GetInventoryLowOfStockCountAsync()
        {
            var lowStockCount = await _getService.GetInventoryLowStockCountAsync();
            if (lowStockCount == 0)
                return NotFound(0);
            return Ok(lowStockCount);
        }
        [HttpGet("InventoryOutOfStock")]
        public async Task<ActionResult<int>> GetInventoryOutOfStockCountAsync()
        {
            var outOfStockCount = await _getService.GetInventoryOutOfStockCountAsync();
            if (outOfStockCount == 0)
                return NotFound(0);
            return Ok(outOfStockCount);
        }
        [HttpGet("TopSellingMonthly")]
        public async Task<ActionResult<List<ProductSalesSummaryDto>>> GetTopSellingProductsMonthlyAsync()
        {
            var topSellingProducts = await _getService.GetTopSellingProductsMonthlyAsync();
            if (!topSellingProducts.Any() || topSellingProducts.Count() == 0)
                return NotFound(new List<ProductSalesSummaryDto>());
            return Ok(topSellingProducts);
        }
        #endregion

        #region StockHistory
        [HttpGet("StockHistory")]
        public async Task<IActionResult> GetStockHistory(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? transactionType = null,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                // Validate dates if both are provided
                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    return BadRequest(new { message = "Start date cannot be after end date" });
                }

                var response = await _getService.GetStockHistoryAsync(
                    startDate,
                    endDate,
                    transactionType,
                    searchTerm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving stock history", error = ex.Message });
            }
        }

        #endregion
    }
}

