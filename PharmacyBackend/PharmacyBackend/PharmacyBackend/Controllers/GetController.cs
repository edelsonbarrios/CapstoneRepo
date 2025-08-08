using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
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
    }
}
