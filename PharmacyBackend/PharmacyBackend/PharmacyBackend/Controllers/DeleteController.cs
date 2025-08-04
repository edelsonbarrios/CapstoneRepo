using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSales(int id)
        {
            var success = await _deleteService.DeleteSalesTransactionByIdAsync(id);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction deleted successfully.");
        }
    }
}
