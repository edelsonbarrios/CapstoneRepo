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

        [HttpDelete("delete/sale")]
        public async Task<IActionResult> DeleteSalesTransactionByIdAsync(int id)
        {
            var deleteSale = await _deleteService.DeleteSalesTransactionByIdAsync(id);
            if (!deleteSale)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction deleted successfully.");
        }

        [HttpDelete("Delete/Discount")]
        public async Task<IActionResult> DeleteDiscountByIdAsync(int id)
        {
            var deleteDiscount = await _deleteService.DeleteDiscountByIdAsync(id);
            if (!deleteDiscount)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("Delete/Product")]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            var deleteProduct = await _deleteService.DeleteProductByIdAsync(id);
            if (!deleteProduct)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("Delete/Category")]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            var deleteCategory = await _deleteService.DeleteCategoryByIdAsync(id);
            if (!deleteCategory)
                return NotFound("Category not found or already deleted.");
            return Ok("Category deleted successfully.");
        }

        //ItemDataControl

        //Brand
        [HttpDelete("Delete/Brand")]
        public async Task<IActionResult> DeleteBrandByIdAsync(int id)
        {
            var deleteBrand = await _deleteService.DeleteBrandByIdAsync(id);
            if (!deleteBrand) return NotFound();
            return NoContent();
        }

        //Generic
        [HttpDelete("Delete/Generic")]
        public async Task<IActionResult> DeleteGenericByIdAsync(int id)
        {
            var deleteGeneric = await _deleteService.DeleteGenericByIdAsync(id);
            if (!deleteGeneric) return NotFound();
            return NoContent();
        }

        //Formulation
        [HttpDelete("Delete/Formulation")]
        public async Task<IActionResult> DeleteFormulationByIdAsync(int id)
        {
            var deleteFormulation = await _deleteService.DeleteFormulationByIdAsync(id);
            if (!deleteFormulation) return NotFound();
            return NoContent();
        }

        //Classification
        [HttpDelete("Delete/Classification")]
        public async Task<IActionResult> DeleteClassificationByIdAsync(int id)
        {
            var deleteClassification = await _deleteService.DeleteClassificationByIdAsync(id);
            if (!deleteClassification) return NotFound();
            return NoContent();
        }

        //Type
        [HttpDelete("Delete/Type")]
        public async Task<IActionResult> DeleteTypeModelByIdAsync(int id)
        {
            var deleteType = await _deleteService.DeleteTypeModelByIdAsync(id);
            if (!deleteType) return NotFound();
            return NoContent();
        }
    }
}

