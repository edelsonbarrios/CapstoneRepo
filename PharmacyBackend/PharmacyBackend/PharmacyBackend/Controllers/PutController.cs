using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;

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
        [HttpPut("update/sale")]
        public async Task<IActionResult> UpdateSalesTransactionAsync(int id, [FromBody] UpdateSalesTransactionDto dto)
        {
            var success = await _updateService.UpdateSalesTransactionAsync(id, dto);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction updated successfully.");
        }

        [HttpPut("Update/Discount")]
        public async Task<IActionResult> UpdateDiscountAsync(int id, [FromBody] UpdateDiscountDto dto)
        {
            var success = await _updateService.UpdateDiscountAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpPut("Update/Product")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto dto)
        {
            var success = await _updateService.UpdateProductAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpPut("Update/Category")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryDto dto)
        {
            var success = await _updateService.UpdateCategoryAsync(id, dto);
            if (!success)
                return NotFound("Category not found or update failed.");
            return Ok("Category updated successfully.");
        }

        //ItemDataControl

        //Brand
        [HttpPut("Update/Brand")]
        public async Task<IActionResult> UpdateBrandAsync(int id, [FromBody] UpdateBrandDto dto)
        {
            var success = await _updateService.UpdateBrandAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        //Generic
        [HttpPut("Update/Generic")]
        public async Task<IActionResult> UpdateGenericAsync(int id, [FromBody] UpdateGenericDto dto)
        {
            var success = await _updateService.UpdateGenericAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        //Formulation
        [HttpPut("Update/Formulation")]
        public async Task<IActionResult> UpdateFormulationAsync(int id, [FromBody] UpdateFormulationDto dto)
        {
            var success = await _updateService.UpdateFormulationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        //Classification
        [HttpPut("Update/Classification")]
        public async Task<IActionResult> UpdateClassificationAsync(int id, [FromBody] UpdateClassificationDto dto)
        {
            var success = await _updateService.UpdateClassificationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        //Type
        [HttpPut("Update/Type")]
        public async Task<IActionResult> UpdateTypeModelAsync(int id, [FromBody] UpdateTypeModelDto dto)
        {
            var success = await _updateService.UpdateTypeModelAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        //Supplier
        [HttpPut("Update/Supplier")]
        public async Task<IActionResult> UpdateSupplierAsync(int id, [FromBody] UpdateSupplierDto dto)
        {
            var success = await _updateService.UpdateSupplierAsync(id, dto);
            if (!success)
                return NotFound("Supplier not found or update failed.");
            return Ok("Supplier updated successfully.");
        }

        //Inventory
        [HttpPut("Update/Inventory")]
        public async Task<IActionResult> UpdateInventoryAsync(int id, [FromBody] UpdateInventoryDto dto)
        {
            var success = await _updateService.UpdateInventoryAsync(id, dto);
            if (!success)
                return NotFound("Inventory not found or update failed.");
            return Ok("Inventory updated successfully.");
        }

        //VAT
        [HttpPut("Update/VAT")]
        public async Task<IActionResult> UpdateVATAsync(int id, [FromBody] UpdateVATDto dto)
        {
            var success = await _updateService.UpdateVATAsync(id, dto);
            if (!success)
                return NotFound("VAT not found or update failed.");
            return Ok("VAT updated successfully.");
        }

        //PurchaseOrder
        [HttpPut("Update/PurchaseOrder")]
        public async Task<IActionResult> UpdatePurchaseOrderAsync(int id, [FromBody] UpdatePurchaseOrderDto dto)
        {
            var success = await _updateService.UpdatePurchaseOrderAsync(id, dto);
            if (!success)
                return NotFound("Purchase Order not found or update failed.");
            return Ok("Purchase Order updated successfully.");
        }
    }
}
