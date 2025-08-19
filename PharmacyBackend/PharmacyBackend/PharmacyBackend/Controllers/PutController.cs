using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
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

        #region Employee
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
        #endregion

        #region Account
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
        #endregion

        #region Sale
        [HttpPut("update/sale")]
        public async Task<IActionResult> UpdateSalesTransactionAsync(int id, [FromBody] UpdateSalesTransactionDto dto)
        {
            var success = await _updateService.UpdateSalesTransactionAsync(id, dto);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction updated successfully.");
        }
        #endregion

        #region Discount
        [HttpPut("Update/Discount")]
        public async Task<IActionResult> UpdateDiscountAsync(int id, [FromBody] UpdateDiscountDto dto)
        {
            var success = await _updateService.UpdateDiscountAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Product
        [HttpPut("Update/Product")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto dto)
        {
            var success = await _updateService.UpdateProductAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Category
        [HttpPut("Update/Category")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryDto dto)
        {
            var success = await _updateService.UpdateCategoryAsync(id, dto);
            if (!success)
                return NotFound("Category not found or update failed.");
            return Ok("Category updated successfully.");
        }
        #endregion

        #region Brand
        [HttpPut("Update/Brand")]
        public async Task<IActionResult> UpdateBrandAsync(int id, [FromBody] UpdateBrandDto dto)
        {
            var success = await _updateService.UpdateBrandAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Generic
        [HttpPut("Update/Generic")]
        public async Task<IActionResult> UpdateGenericAsync(int id, [FromBody] UpdateGenericDto dto)
        {
            var success = await _updateService.UpdateGenericAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Formulation
        [HttpPut("Update/Formulation")]
        public async Task<IActionResult> UpdateFormulationAsync(int id, [FromBody] UpdateFormulationDto dto)
        {
            var success = await _updateService.UpdateFormulationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Classification
        [HttpPut("Update/Classification")]
        public async Task<IActionResult> UpdateClassificationAsync(int id, [FromBody] UpdateClassificationDto dto)
        {
            var success = await _updateService.UpdateClassificationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Type
        [HttpPut("Update/Type")]
        public async Task<IActionResult> UpdateTypeModelAsync(int id, [FromBody] UpdateTypeModelDto dto)
        {
            var success = await _updateService.UpdateTypeModelAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion
    }
}
