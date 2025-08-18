 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.Models;
using System.Diagnostics.Contracts;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private readonly IGetServiceContracts _getService;

        public GetController(IGetServiceContracts getService)
        {
            _getService = getService;
        }

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
        // ---------------- SALES ----------------
        [HttpGet("SaleAll")]
        public async Task<ActionResult<IEnumerable<SalesTransactionDto>>> GetAllSalesTransactionsAsync()
        {
            var sales = await _getService.GetAllSalesTransactionsAsync();
            if (sales == null || !sales.Any())
                return NotFound(new { Message = "No sales transactions found." });

            return Ok(sales);
        }

        [HttpGet("Sale")]
        public async Task<ActionResult<SalesTransactionDto>> GetSalesTransactionByIdAsync(int id)
        {
            var sale = await _getService.GetSalesTransactionByIdAsync(id);
            if (sale == null)
                return NotFound(new { Message = "Sales transaction not found." });

            return Ok(sale);
        }

        // ---------------- DISCOUNT ----------------
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

        // ---------------- PRODUCT ----------------
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

        // ---------------- CATEGORY ----------------
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

        // ---------------- BRAND ----------------
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

        // ---------------- GENERIC ----------------
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

        // ---------------- FORMULATION ----------------
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

        // ---------------- CLASSIFICATION ----------------
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

        // ---------------- TYPE ----------------
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
        //Supplier
        [HttpGet("SupplierAll")]
        public async Task<IActionResult> GetAllSupplierAsync()
        {
            var suppliers = await _getService.GetAllSupplierAsync();
            if (!suppliers.Any())
                return NotFound("No suppliers found.");
            return Ok(suppliers);
        }

        [HttpGet("SupplierId")]
        public async Task<IActionResult> GetSupplierByIdAsync(int id)
        {
            var supplier = await _getService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound("Supplier not found.");
            return Ok(supplier);
        }

        //Inventory
        [HttpGet("InventoryAll")]
        public async Task<IActionResult> GetAllInventoryAsync()
        {
            var inventories = await _getService.GetAllInventoryAsync();
            if (!inventories.Any())
                return NotFound("No inventory found.");
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

        //VAT
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

        //PurchaseOrder
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

        //StockMovement
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
    }
}

