using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly IDeleteServiceContracts _deleteService;

        public DeleteController(IDeleteServiceContracts deleteService)
        {
            _deleteService = deleteService;
        }
        [HttpDelete("Employee")]
        public async Task<ActionResult<EmployeeRequestResponse>> DeleteEmployeeByIdAsync(int id)
        {
            var deleteEmployee = await _deleteService.DeleteEmployeeByIdAsync(id);
            if (!deleteEmployee)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be deleted." });
            return Ok(new EmployeeRequestResponse { Message = "Employee deleted." });
        }
        [HttpDelete("Account")]
        public async Task<ActionResult<AccountRequestResponse>> DeleteAccountByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest(new AccountRequestResponse { Message = "Invalid account. Please try again." });
            var deleteAccount = await _deleteService.DeleteAccountByIdAsync(id);
            if (!deleteAccount)
                return BadRequest(new AccountRequestResponse { Message = "Account cannot be deleted." });
            return Ok(new AccountRequestResponse { Message = "Account successfully deleted." });
        }
    }
}
