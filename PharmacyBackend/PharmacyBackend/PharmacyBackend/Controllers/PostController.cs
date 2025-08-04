using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.PostgresTypes;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.DTOs.EmployeeDTOs;
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

        [HttpPost("sale")]
        public async Task<IActionResult> CreateSalesTransaction([FromBody] CreateSalesTransactionDto dto)
        {
            var result = await _postService.CreateSalesTransactionAsync(dto);
            return Ok(result);
        }
    }
}
