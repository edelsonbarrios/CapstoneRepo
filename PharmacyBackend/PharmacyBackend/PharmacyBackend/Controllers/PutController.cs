using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PutController : ControllerBase
    {
        private readonly IUpdateServiceContracts _updateService;

        public PutController(IUpdateServiceContracts updateService)
        {
            _updateService = updateService;
        }

        [HttpPut("Employee")]
        public async Task<ActionResult<EmployeeRequestResponse>> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be updated." });
            var updateEmployee = await _updateService.UpdateEmployeeAsync(request);
            if (!updateEmployee)
                return BadRequest(new EmployeeRequestResponse { Message = "Failed to updated employee." });
            return Ok(new EmployeeRequestResponse { Message = "Employee updated sucessfully." });
        }
        [HttpPut("Account")]
        public async Task<ActionResult<UpdateAccountResponse>> UpdateAccountAsync(UpdateAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new UpdateAccountResponse { Message = "Account invalid." });

            var updateAccount = await _updateService.UpdateAccountAsync(request);
            if (!updateAccount)
                return BadRequest(new UpdateAccountResponse { Message = "Account cannot be updated. Please try again." });

            return Ok(new UpdateAccountResponse { Message = "Account successfully updated." });
        }
    }
}
