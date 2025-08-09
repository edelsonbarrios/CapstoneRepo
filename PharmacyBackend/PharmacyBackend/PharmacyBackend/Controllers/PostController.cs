using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.PostgresTypes;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.SalesDTOs;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostServiceContracts _postService;

        public PostController(IPostServiceContracts postService)
        {
            _postService = postService;
        }
        [HttpPost("Employee")]
        public async Task<ActionResult<EmployeeRequestResponse>> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid properties.");
            var createEmployee = await _postService.CreateEmployeeAsync(request);
            if (!createEmployee)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be created. Please try again." });
            return Ok(
                new EmployeeRequestResponse { Message = $"Employee successfully created. " });
        }
        //salesController
        [HttpPost("sale")]
        public async Task<IActionResult> CreateSalesTransactionAsync([FromBody] CreateSalesTransactionDto dto)
        {
            var result = await _postService.CreateSalesTransactionAsync(dto);
            return Ok(result);
        }

        //DiscountController
        [HttpPost("Discount")]
        public async Task<IActionResult> CreateDiscountAsync([FromBody] CreateDiscountDto dto)
        {
            var result = await _postService.CreateDiscountAsync(dto);
            return Ok(result);
        }

        [HttpPost("Product")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto dto)
        {
            var result = await _postService.CreateProductAsync(dto);

            return Ok(result);

        }

        [HttpPost("Category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto dto)
        {
            var result = await _postService.CreateCategoryAsync(dto);
            return Ok("Category created successfully.");
        }

    }
}
