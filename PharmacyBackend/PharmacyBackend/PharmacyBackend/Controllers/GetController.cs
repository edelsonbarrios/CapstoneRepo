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

        #region EmployeeController
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
        #endregion


        #region AccountController
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
        #endregion


        //SALE
        #region Sales
        [HttpGet("SaleAll")]
        public async Task<ActionResult<IEnumerable<SalesTransactionDto>>> GetAllSalesTransactionsAsync()
        {
            var sales = await _getService.GetAllSalesTransactionsAsync();
            if (!sales.Any())
                return NotFound("No sales transactions found.");
            return Ok(sales);
        }

        [HttpGet("SalesId")]
        public async Task<ActionResult<SalesTransactionDto>> GetSalesTransactionByIdAsync(int id)
        {
            var sale = await _getService.GetSalesTransactionByIdAsync(id);
            if (sale == null)
                return NotFound("Sales transaction not found.");
            return Ok(sale);
        }
        #endregion

        //DISCOUNT
        #region Discount 
        [HttpGet("DiscountAll")]
        public async Task<IActionResult> GetAllDiscountsAsync()
        {
            var discount = await _getService.GetAllDiscountAsync();
            return Ok(discount);
        }

        // GET: api/Discount/5
        [HttpGet("DiscountId")]
        public async Task<IActionResult> GetDiscountByIdAsync(int id)
        {
            var discount = await _getService.GetDiscountByIdAsync(id);
            if (discount == null)
                return NotFound("Discount not found");
            return Ok(discount);
        }
        #endregion

        //Product
        #region Product
        [HttpGet("ProductAll")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var products = await _getService.GetAllProductAsync();
            return Ok(products);
        }

        [HttpGet("ProductId")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _getService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        #endregion

        //Category
        #region Category
        [HttpGet("CategoryAll")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var Category = await _getService.GetAllCategoryAsync();
            if (!Category.Any())
                return NotFound("No categories found.");
            return Ok(Category);
        }

        [HttpGet("CategoryId")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var Category = await _getService.GetCategoryByIdAsync(id);
            if (Category == null)
                return NotFound("Category not found.");
            return Ok(Category);
        }
        #endregion

        //ItemDataControl
        //Brand
        #region Brand
        [HttpGet("BrandAll")]
        public async Task<IActionResult> GetAllBrandAsync()
        {
            var brand = await _getService.GetAllBrandAsync();
            if (!brand.Any())
                return NotFound("No Brand found.");
            return Ok(brand);
        }

        [HttpGet("BrandId")]
        public async Task<IActionResult> GetBrandByIdAsync(int id)
        {
            var Brand = await _getService.GetBrandByIdAsync(id);
            if (Brand == null)
                return NotFound("Brand Not Found");
            return Ok(Brand);
        }
        #endregion

        //Generic
        #region Generic
        [HttpGet("GenericAll")]
        public async Task<IActionResult> GetAllGenericAsync()
        {
            var generic = await _getService.GetAllGenericAsync();
            if (!generic.Any())
                return NotFound("No Brand found.");
            return Ok(generic);
        }

        [HttpGet("GenericId")]
        public async Task<IActionResult> GetGenericByIdAsync(int id)
        {
            var generic = await _getService.GetBrandByIdAsync(id);
            if (generic == null)
                return NotFound("Brand Not Found");
            return Ok(generic);
        }
        #endregion

        //Formulation
        #region Formulation
        [HttpGet("FormulationAll")]
        public async Task<IActionResult> GetAllFormulationAsync()
        {
            var formulation = await _getService.GetAllFormulationAsync();
            if (!formulation.Any())
                return NotFound("No Brand found.");
            return Ok(formulation);
        }

        [HttpGet("FormulationId")]
        public async Task<IActionResult> GetFormulationByIdAsync(int id)
        {
            var formulation = await _getService.GetFormulationByIdAsync(id);
            if (formulation == null)
                return NotFound("Brand Not Found");
            return Ok(formulation);
        }
        #endregion

        //Classification
        #region Classification
        [HttpGet("ClassificationAll")]
        public async Task<IActionResult> GetAllClassificationAsync()
        {
            var classification = await _getService.GetAllClassificationAsync();
            if (!classification.Any())
                return NotFound("No Brand found.");
            return Ok(classification);
        }

        [HttpGet("ClassificationId")]
        public async Task<IActionResult> GetClassificationByIdAsync(int id)
        {
            var classification = await _getService.GetClassificationByIdAsync(id);
            if (classification == null)
                return NotFound("Brand Not Found");
            return Ok(classification);
        }
        #endregion

        //Type
        #region Type
        [HttpGet("TypeAll")]
        public async Task<IActionResult> GetAllTypeModelAsync()
        {
            var type = await _getService.GetAllTypeModelAsync();
            if (!type.Any())
                return NotFound("No Brand found.");
            return Ok(type);
        }

        [HttpGet("TypeId")]
        public async Task<IActionResult> GetTypeModelByIdAsync(int id)
        {
            var type = await _getService.GetTypeModelByIdAsync(id);
            if (type == null)
                return NotFound("Brand Not Found");
            return Ok(type);
        }
        #endregion
    }
}

