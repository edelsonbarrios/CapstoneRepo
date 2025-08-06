using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql.PostgresTypes;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;

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
        [HttpPost]
        public async Task<ActionResult<LogInResponse>> AuthenticationAsync(LogInRequest request)
        {
            if (request is null)
                return new LogInResponse { Token = null, Message = "Invalid Username or Password." };
            var authentication = await _postService.AuthenticationAsync(request);
            if (!authentication)
                return new LogInResponse { Token = null, Message = "Wrong username or password." };
        }
    }
}
