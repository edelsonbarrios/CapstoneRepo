using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.PostgresTypes;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
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
        private readonly IGetServiceContracts _getService;

        public PostController(IPostServiceContracts postService,
            IGetServiceContracts getService)
        {
            _postService = postService;
            _getService = getService;
        }
        [HttpPost("Employee")]
        public async Task<ActionResult<EmployeeRequestResponse>> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid properties.");
            var employeeId = await _postService.CreateEmployeeAsync(request);
            if (employeeId == 0)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be created. Please try again." });
            return Ok(new EmployeeRequestResponse { Message = $"Employee successfully created. " });
        }
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
    }
}
