using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.PostgresTypes;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostServiceContracts _postService;
        private readonly IGetServiceContracts _getService;

        public PostController(IPostServiceContracts postService,
            IGetServiceContracts getService)
        {
            _postService = postService;
            _getService = getService;
        }

        #region Employee
        [HttpPost("Employee")]
        public async Task<ActionResult<CreateEmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid properties.");
            var employeeId = await _postService.CreateEmployeeAsync(request);
            if (employeeId == 0)
                return BadRequest(new CreateEmployeeResponse { Message = "Employee cannot be created. Please try again.", EmployeeId = employeeId });
            return Ok(new CreateEmployeeResponse { Message = $"Employee successfully created. ", EmployeeId = employeeId });
        }
        #endregion

        #region Account
        [HttpPost("Login")]
        public async Task<ActionResult<LogInResponse>> AuthenticationAsync(LogInRequest request)
        {
            if (request is null)
                return new LogInResponse { Token = null, Message = "Invalid Username or Password." };
            var token = await _postService.AuthenticationAsync(request);
            if (token is null)
                return new LogInResponse { Token = null, Message = "Wrong username or password." };
            return Ok(new LogInResponse { Token = token, Message = "Successfully logged in." });
        }
        [HttpPost("Account")]
        public async Task<ActionResult<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CreateAccountResponse { Message = "Invalid Username or Password." });

            var userNameValidation = await _getService.AccountUsernameValidation(request.Username);
            if (userNameValidation)
                return BadRequest(new CreateAccountResponse { Message = "Username is already taken." });

            var createAccount = await _postService.CreateAccountAsync(request);
            if (!createAccount)
                return BadRequest(new CreateAccountResponse { Message = "Account cannot be created." });

            return Ok(new CreateAccountResponse { Message = "Account successfully created." });
        }
        #endregion


        #region Sales
        [HttpPost("sale")]
        public async Task<IActionResult> CreateSalesTransactionAsync([FromBody] CreateSalesTransactionDto dto)
        {
            var result = await _postService.CreateSalesTransactionAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Discount
        [HttpPost("Discount")]
        public async Task<IActionResult> CreateDiscountAsync([FromBody] CreateDiscountDto dto)
        {
            var result = await _postService.CreateDiscountAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Product
        [HttpPost("Product")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto dto)
        {
            var result = await _postService.CreateProductAsync(dto);

            return Ok(result);

        }
        #endregion

        #region Category
        [HttpPost("Category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto dto)
        {
            var result = await _postService.CreateCategoryAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Brand
        [HttpPost("Brand")]
        public async Task<IActionResult> CreateBrandAsync([FromBody] CreateBrandDto dto)
        {
            var result = await _postService.CreateBrandAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Generic
        [HttpPost("Generic")]
        public async Task<IActionResult> CreateGenericAsync([FromBody] CreateGenericDto dto)
        {
            var result = await _postService.CreateGenericAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Formulation
        [HttpPost("Formulation")]
        public async Task<IActionResult> CreateFormulationAsync([FromBody] CreateFormulationDto dto)
        {
            var result = await _postService.CreateFormulationAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Classification
        [HttpPost("Classification")]
        public async Task<IActionResult> CreateClassificationAsync([FromBody] CreateClassificationDto dto)
        {
            var result = await _postService.CreateClassificationAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Type
        [HttpPost("Type")]
        public async Task<IActionResult> CreateTypeModelAsync([FromBody] CreateTypeModelDto dto)
        {
            var result = await _postService.CreateTypeModelAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Supplier
        [HttpPost("Supplier")]
        public async Task<IActionResult> CreateSupplierAsync([FromBody] CreateSupplierDto dto)
        {
            var result = await _postService.CreateSupplierAsync(dto);
            return Ok(result);
        }
        #endregion

        #region Inventory
        [HttpPost("Inventory")]
        public async Task<IActionResult> CreateInventoryAsync([FromBody] CreateInventoryDto dto)
        {
            var result = await _postService.CreateInventoryAsync(dto);
            return Ok(result);
        }
        #endregion

        #region VAT
        [HttpPost("VAT")]
        public async Task<IActionResult> CreateVATAsync([FromBody] CreateVATDto dto)
        {
            var result = await _postService.CreateVATAsync(dto);
            return Ok(result);
        }
        #endregion

        #region PurchaseOrder
        [HttpPost("PurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrderAsync([FromBody] CreatePurchaseOrderDto dto)
        {
            var result = await _postService.CreatePurchaseOrderAsync(dto);
            return Ok(result);
        }
        #endregion

        #region StockMovement
        [HttpPost("StockMovement")]
        public async Task<IActionResult> CreateStockMovementAsync([FromBody] CreateStockMovementDto dto)
        {
            try
            {
                var result = await _postService.CreateStockMovementAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}