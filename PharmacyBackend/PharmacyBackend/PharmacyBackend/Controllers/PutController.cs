using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;

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

        [HttpPut]
        public async Task<ActionResult<EmployeeRequestResponse>> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be updated." });
            var updateEmployee = await _updateService.UpdateEmployeeAsync(request);
            if (!updateEmployee)
                return BadRequest(new EmployeeRequestResponse { Message = "Failed to updated employee." });
            return Ok(new EmployeeRequestResponse { Message = "Employee updated sucessfully." });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSales(int id, [FromBody] UpdateSalesTransactionDto dto)
        {
            var success = await _updateService.UpdateSalesTransactionAsync(id, dto);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction updated successfully.");
        }
    }
}
