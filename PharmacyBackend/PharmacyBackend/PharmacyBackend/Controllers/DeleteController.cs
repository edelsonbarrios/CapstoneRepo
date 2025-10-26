using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly IDeleteServiceContracts _deleteService;
        private readonly ILogger<DeleteController> _logger;

        public DeleteController(IDeleteServiceContracts deleteService, ILogger<DeleteController> logger)
        {
            _deleteService = deleteService;
            _logger = logger;
        }
        #region Employee
        [HttpDelete("Employee")]
        public async Task<ActionResult<EmployeeRequestResponse>> DeleteEmployeeByIdAsync(int id)
        {
            var deleteEmployee = await _deleteService.DeleteEmployeeByIdAsync(id);
            if (!deleteEmployee)
                return BadRequest(new EmployeeRequestResponse { Message = "Employee cannot be deleted." });
            return Ok(new EmployeeRequestResponse { Message = "Employee deleted." });
        }
        #endregion

        #region Account
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
        #endregion

        #region Sales
        [HttpDelete("Transaction/{id}")]
        public async Task<IActionResult> DeleteSalesTransactionByIdAsync(int id)
        {
            var success = await _deleteService.DeleteSalesTransactionByIdAsync(id);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction deleted successfully.");
        }

        [HttpDelete("Item/{id}")]
        public async Task<IActionResult> DeleteSalesItemAsync(int id)
        {
            var success = await _deleteService.DeleteSalesItemAsync(id);
            if (!success)
                return NotFound($"No sales item found with ID {id}");

            return Ok("Sales item deleted successfully.");
        }
        #endregion

        #region Discount
        [HttpDelete("Delete/Discount")]
        public async Task<IActionResult> DeleteDiscountByIdAsync(int id)
        {
            var deleteDiscount = await _deleteService.DeleteDiscountByIdAsync(id);
            if (!deleteDiscount)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Product
        [HttpDelete("Delete/Product")]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            var deleteProduct = await _deleteService.DeleteProductByIdAsync(id);
            if (!deleteProduct)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Category
        [HttpDelete("Delete/Category")]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            var deleteCategory = await _deleteService.DeleteCategoryByIdAsync(id);
            if (!deleteCategory)
                return NotFound("Category not found or already deleted.");
            return Ok("Category deleted successfully.");
        }
        #endregion

        #region Brand
        [HttpDelete("Delete/Brand")]
        public async Task<IActionResult> DeleteBrandByIdAsync(int id)
        {
            var deleteBrand = await _deleteService.DeleteBrandByIdAsync(id);
            if (!deleteBrand) return NotFound();
            return NoContent();
        }
        #endregion

        #region Generic
        [HttpDelete("Delete/Generic")]
        public async Task<IActionResult> DeleteGenericByIdAsync(int id)
        {
            var deleteGeneric = await _deleteService.DeleteGenericByIdAsync(id);
            if (!deleteGeneric) return NotFound();
            return NoContent();
        }
        #endregion

        #region Formulation
        [HttpDelete("Delete/Formulation")]
        public async Task<IActionResult> DeleteFormulationByIdAsync(int id)
        {
            var deleteFormulation = await _deleteService.DeleteFormulationByIdAsync(id);
            if (!deleteFormulation) return NotFound();
            return NoContent();
        }
        #endregion

        #region Classification
        [HttpDelete("Delete/Classification")]
        public async Task<IActionResult> DeleteClassificationByIdAsync(int id)
        {
            var deleteClassification = await _deleteService.DeleteClassificationByIdAsync(id);
            if (!deleteClassification) return NotFound();
            return NoContent();
        }
        #endregion

        #region Type
        [HttpDelete("Delete/Type")]
        public async Task<IActionResult> DeleteTypeModelByIdAsync(int id)
        {
            var deleteType = await _deleteService.DeleteTypeModelByIdAsync(id);
            if (!deleteType) return NotFound();
            return NoContent();
        }
        #endregion

        #region Supplier
        [HttpDelete("Delete/Supplier")]
        public async Task<IActionResult> DeleteSupplierByIdAsync(int id)
        {
            var deleteSupplier = await _deleteService.DeleteSupplierByIdAsync(id);
            if (!deleteSupplier)
                return NotFound("Supplier not found or already deleted.");
            return Ok("Supplier deleted successfully.");
        }
        #endregion

        #region Inventory
        [HttpDelete("Delete/Inventory")]
        public async Task<IActionResult> DeleteInventoryByIdAsync(int id)
        {
            var deleteInventory = await _deleteService.DeleteInventoryByIdAsync(id);
            if (!deleteInventory)
                return NotFound("Inventory not found or already deleted.");
            return Ok("Inventory deleted successfully.");
        }
        #endregion

        #region VAT
        [HttpDelete("Delete/VAT")]
        public async Task<IActionResult> DeleteVATByIdAsync(int id)
        {
            var deleteVAT = await _deleteService.DeleteVATByIdAsync(id);
            if (!deleteVAT)
                return NotFound("VAT not found or already deleted.");
            return Ok("VAT deleted successfully.");
        }
        #endregion

        #region PurchaseOrder
        [HttpDelete("Delete/PurchaseOrder")]
        public async Task<IActionResult> DeletePurchaseOrderByIdAsync(int id)
        {
            var deletePurchaseOrder = await _deleteService.DeletePurchaseOrderByIdAsync(id);
            if (!deletePurchaseOrder)
                return NotFound("Purchase Order not found or already deleted.");
            return Ok("Purchase Order deleted successfully.");
        }
        #endregion

        #region BackUp Database

        /// <summary>
        /// Delete a specific backup file
        /// </summary>
        [HttpDelete("backups/{fileName}")]
        //[Authorize] // Add role if needed: [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteBackupResponse>> DeleteBackup(string fileName)
        {
            try
            {
                // Validate filename to prevent directory traversal attacks
                if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                {
                    return BadRequest(new DeleteBackupResponse
                    {
                        Success = false,
                        Message = "Invalid filename"
                    });
                }

                _logger.LogInformation($"Backup deletion requested: {fileName}");
                var response = await _deleteService.DeleteBackupAsync(fileName);

                if (response.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteBackup endpoint");
                return StatusCode(500, new DeleteBackupResponse
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Manually trigger cleanup of old backups
        /// </summary>
        [HttpDelete("backups/cleanup")]
        //[Authorize] // Add role if needed: [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CleanupOldBackups()
        {
            try
            {
                _logger.LogInformation("Manual backup cleanup requested");
                await _deleteService.CleanupOldBackupsAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Cleanup completed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CleanupOldBackups endpoint");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}"
                });
            }
        }

        #endregion
    }
}

