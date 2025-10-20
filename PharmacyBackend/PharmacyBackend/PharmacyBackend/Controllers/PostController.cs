using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens.Experimental;
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
using System.Linq.Expressions;

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
                return BadRequest(new EmployeeRequestResponse { Message = "Invalid properties." });

            try
            {
                var employeeId = await _postService.CreateEmployeeAsync(request);
                if (employeeId == 0)
                    return BadRequest(new CreateEmployeeResponse { Message = "Employee cannot be created. Please try again.", EmployeeId = employeeId });
                return Ok(new CreateEmployeeResponse { Message = $"Employee successfully created. ", EmployeeId = employeeId });
            }catch(Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }
        #endregion

        #region Account
        [HttpPost("Login")]
        public async Task<ActionResult<LogInResponse>> AuthenticationAsync(LogInRequest request)
        {
            if (request is null)
                return BadRequest(new LogInResponse { Token = null, Message = "Invalid Username or Password." });

            var token = await _postService.AuthenticationAsync(request);
            if (token is null)
                return NotFound(new LogInResponse { Token = null, Message = "Wrong username or password." });

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
        [HttpPost("Transaction")]
        public async Task<IActionResult> CreateSalesTransactionAsync([FromBody] CreateSalesTransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid sales transaction data.");

            if (dto == null)
                return BadRequest("Sales transaction data is required.");

            try
            {
                var result = await _postService.CreateSalesTransactionAsync(dto);
                if (result == null)
                    return BadRequest("Sales transaction could not be created.");

                return Ok(new { Message = "Transaction successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating sales transaction: {ex.Message}");
            }
        }

        [HttpPost("Item")]
        public async Task<IActionResult> CreateSalesItemAsync([FromBody] CreateSalesItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid sales item data.");

            try
            {
                var result = await _postService.CreateSalesItemAsync(dto);
                if (result == null)
                    return BadRequest("Sales item could not be created.");

                return Ok(new { Message = "Sales item successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating sales item: {ex.Message}");
            }
        }
        #endregion

        #region Discount
        [HttpPost("Discount")]
        public async Task<IActionResult> CreateDiscountAsync([FromBody] CreateDiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid discount data.");

            if (dto == null)
                return BadRequest("Discount data is required.");

            try
            {
                var result = await _postService.CreateDiscountAsync(dto);
                if (result == null)
                    return BadRequest("Discount could not be created.");

                return Ok(new { Message = "Discount successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating discount: {ex.Message}");
            }
        }

       

        #endregion

        #region Product
        [HttpPost("Product")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid product data.");

            if (dto == null)
                return BadRequest("Product data is required.");

            try
            {
                var result = await _postService.CreateProductAsync(dto);
                if (result == null)
                    return BadRequest("Product could not be created.");

                return Ok(new { Message = "Product successfully created." });
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error creating product: {ex.Message}");
            } 
        }
        #endregion

        #region Category
        [HttpPost("Category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid category data.");

            if (dto == null)
                return BadRequest("Category data is required.");

            try
            {
                var result = await _postService.CreateCategoryAsync(dto);
                if (result == null)
                    return BadRequest("Category could not be created.");

                return Ok(new { Message = "Category successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating category: {ex.Message}");
            }
        }
        #endregion

        #region Brand
        [HttpPost("Brand")]
        public async Task<IActionResult> CreateBrandAsync([FromBody] CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid brand data.");

            if (dto == null)
                return BadRequest("Brand data is required.");

            try
            {
                var result = await _postService.CreateBrandAsync(dto);
                if (result == null)
                    return BadRequest("Brand could not be created.");

                return Ok(new { Message = "Brand successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating brand: {ex.Message}");
            }
        }
        #endregion

        #region Generic
        [HttpPost("Generic")]
        public async Task<IActionResult> CreateGenericAsync([FromBody] CreateGenericDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid generic data.");

            if (dto == null)
                return BadRequest("Generic data is required.");

            try
            {
                var result = await _postService.CreateGenericAsync(dto);
                if (result == null)
                    return BadRequest("Generic could not be created.");

                return Ok(new { Message = "Generic successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating generic: {ex.Message}");
            }
        }
        #endregion

        #region Formulation
        [HttpPost("Formulation")]
        public async Task<IActionResult> CreateFormulationAsync([FromBody] CreateFormulationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid formulation data.");

            if (dto == null)
                return BadRequest("Formulation data is required.");

            try
            {
                var result = await _postService.CreateFormulationAsync(dto);
                if (result == null)
                    return BadRequest("Formulation could not be created.");

                return Ok(new { Message = "Formulation successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating formulation: {ex.Message}");
            }
        }
        #endregion

        #region Classification
        [HttpPost("Classification")]
        public async Task<IActionResult> CreateClassificationAsync([FromBody] CreateClassificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid classification data.");

            if (dto == null)
                return BadRequest("Classification data is required.");

            try
            {
                var result = await _postService.CreateClassificationAsync(dto);
                if (result == null)
                    return BadRequest("Classification could not be created.");

                return Ok(new { Message = "Classification successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating classification: {ex.Message}");
            }
        }
        #endregion

        #region Type
        [HttpPost("Type")]
        public async Task<IActionResult> CreateTypeModelAsync([FromBody] CreateTypeModelDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid type data.");

            if (dto == null)
                return BadRequest("Type data is required.");

            try
            {
                var result = await _postService.CreateTypeModelAsync(dto);
                if (result == null)
                    return BadRequest("Type could not be created.");

                return Ok(new { Message = "Type successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating type: {ex.Message}");
            }
        }
        #endregion

        #region Supplier
        [HttpPost("Supplier")]
        public async Task<IActionResult> CreateSupplierAsync([FromBody] CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid supplier data.");

            if (dto == null)
                return BadRequest("Supplier data is required.");

            try
            {
                var result = await _postService.CreateSupplierAsync(dto);
                if (!result)
                    return BadRequest("Supplier could not be created.");

                return Ok(new { Message = "Supplier successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating supplier: {ex.Message}");
            }
        }
        #endregion

        #region Inventory
        [HttpPost("Inventory")]
        public async Task<ActionResult<bool>> CreateInventoryAsync(CreateInventoryDto dto)
        {
             if (!ModelState.IsValid)
                 return BadRequest("Invalid inventory data.");

             if (dto == null)
                 return BadRequest("Inventory data is required.");

             try
             {
                 var result = await _postService.CreateInventoryAsync(dto);
                 if (!result)
                     return BadRequest("Inventory could not be created.");

                 return Ok(new { Message = "Inventory successfully created." });
             }
             catch (Exception ex)
             {
                 return BadRequest($"Error creating inventory: {ex.Message}");
             }
        }

        [HttpPost("StockAdjustment")]
        public async Task<IActionResult> AdjustInventoryStock([FromBody] StockAdjustmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid stock adjustment data.");

            if (dto == null)
                return BadRequest("Stock adjustment data is required.");

            try
            {
                var result = await _postService.AdjustInventoryStockAsync(dto);
                if (!result)
                    return BadRequest("Stock adjustment could not be completed.");

                return Ok(new { Message = "Stock adjustment completed successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid data: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing stock adjustment: {ex.Message}");
            }
        }
        #endregion

        #region VAT
        [HttpPost("VAT")]
        public async Task<IActionResult> CreateVATAsync([FromBody] CreateVATDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid VAT data.");

            if (dto == null)
                return BadRequest("VAT data is required.");
            var validation = await _postService.VatNameValidationAsync(dto.VATName);
            if (!validation)
                return BadRequest("VAT already exist.");
            try
            {
                var result = await _postService.CreateVATAsync(dto);
                if (result == null)
                    return BadRequest("VAT could not be created.");

                return Ok(new { Message = "VAT successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating VAT: {ex.Message}");
            }
        }
        #endregion

        #region PurchaseOrder
        [HttpPost("PurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrderAsync([FromBody] CreatePurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid purchase order data.");

            if (dto == null)
                return BadRequest("Purchase order data is required.");

            try
            {
                var result = await _postService.CreatePurchaseOrderAsync(dto);
                if (!result)
                    return BadRequest("Purchase order could not be created.");

                return Ok(new { Message = "Purchase order successfully created." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating purchase order: {ex.Message}");
            }
        }
        [HttpPost("NewPONumber")]
        public async Task<IActionResult> GenerateNewPurchaseOrderNumberAsync()
        {
            var newPurchaseOrderNumber = await _postService.GenerateNewPurchaseOrderNumberAsync();
            if(string.IsNullOrEmpty(newPurchaseOrderNumber)) {
               return BadRequest("Cannot generate a new invoice number.");
            }
            return Content(newPurchaseOrderNumber);
        }
        #endregion

        #region StockMovement
        [HttpPost("StockMovement")]
        public async Task<IActionResult> CreateStockMovementAsync(CreateStockMovementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid stock movement data.");

            if (dto == null)
                return BadRequest("Stock movement data is required.");

            try
            {
                var result = await _postService.CreateStockMovementAsync(dto);
                if (result == null)
                    return BadRequest("Stock movement could not be created.");

                return Ok(new { Message = "Stock movement successfully created." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating stock movement: {ex.Message}");
            }
        }
        #endregion
    }
}