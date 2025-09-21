using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PharmacyBackend.Contracts.ServiceContracts.UpdateInterface;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using System.Diagnostics.Contracts;

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
        [HttpPut("ArchiveEmployee")]
        public async Task<ActionResult<ArchiveEmployeeResponse>> ArchiveEmployeeByIdAsync(ArchiveEmployeeHistoryRequest request)
        {
            if (request is null)
                return BadRequest(new ArchiveEmployeeResponse { Message = "Employee does not exist."});
            var archiveEmployee = await _updateService.ArchiveEmployeeByIdAsync(request);
            if (!archiveEmployee)
                return BadRequest(new ArchiveEmployeeResponse { Message = "Employee cannot be inactive"});
            return Ok(new ArchiveEmployeeResponse { Message = "Employee is now inactive."});
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
        [HttpPut("Transaction/{id}")]
        public async Task<IActionResult> UpdateSalesTransactionAsync(int id, [FromBody] UpdateSalesTransactionDto dto)
        {
            var success = await _updateService.UpdateSalesTransactionAsync(id, dto);
            if (!success)
                return NotFound($"No sales transaction found with ID {id}");

            return Ok("Sales transaction updated successfully.");
        }
        #endregion

        #region Discount
        [HttpPut("Discount")]
        public async Task<IActionResult> UpdateDiscountAsync(int id, [FromBody] UpdateDiscountDto dto)
        {
            var success = await _updateService.UpdateDiscountAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        [HttpPut("ArchiveDiscount")]
        public async Task<ActionResult<ArchiveDiscountResponse>> ArchiveDiscountByIdAsync(int discountId)
        {
            if (discountId == 0)
                return BadRequest(new ArchiveDiscountResponse { Message = "Discount does not exist." });

            var archiveDiscount = await _updateService.ArchiveDiscountByIdAsync(discountId);
            if (!archiveDiscount)
                return BadRequest(new ArchiveDiscountResponse { Message = "Discount cannot be archived." });

            return Ok(new ArchiveDiscountResponse { Message = "Discount is now inactive" });
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
        [HttpPut("Brand")]
        public async Task<IActionResult> UpdateBrandAsync(int id, [FromBody] UpdateBrandDto dto)
        {
            var success = await _updateService.UpdateBrandAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Generic
        [HttpPut("Generic")]
        public async Task<IActionResult> UpdateGenericAsync(int id,  [FromBody]UpdateGenericDto dto)
        {
            var success = await _updateService.UpdateGenericAsync(id, dto);
            if (!success)
                return NotFound();
            return Ok();
        }
        #endregion

        #region Formulation
        [HttpPut("Formulation")]
        public async Task<IActionResult> UpdateFormulationAsync(int id, [FromBody] UpdateFormulationDto dto)
        {
            var success = await _updateService.UpdateFormulationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Classification
        [HttpPut("Classification")]
        public async Task<IActionResult> UpdateClassificationAsync(int id, [FromBody] UpdateClassificationDto dto)
        {
            var success = await _updateService.UpdateClassificationAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Type
        [HttpPut("Type")]
        public async Task<IActionResult> UpdateTypeModelAsync(int id, [FromBody] UpdateTypeModelDto dto)
        {
            var success = await _updateService.UpdateTypeModelAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        #endregion

        #region Supplier
        [HttpPut("Supplier")]
        public async Task<IActionResult> UpdateSupplierAsync(int id, [FromBody] UpdateSupplierDto dto)
        {
            var success = await _updateService.UpdateSupplierAsync(id, dto);
            if (!success)
                return NotFound("Supplier not found or update failed.");
            return Ok("Supplier updated successfully.");
        }
        [HttpPut("ArchiveSupplier")]
        public async Task<ActionResult<ArchiveSupplierResponse>> ArchiveSupplierByIdAsync(int supplierId)
        {
            if (supplierId == 0)
                return BadRequest(new ArchiveSupplierResponse { Message = "Supplier does not exist." });
            var archiveSupplier = await _updateService.ArchiveSupplierByIdAsync(supplierId);
            if (!archiveSupplier)
                return BadRequest(new ArchiveSupplierResponse { Message = "Supplier cannot be archived." });
            return Ok(new ArchiveSupplierResponse { Message = "Supplier is now inactive" });
        }
        #endregion

        #region Inventory
        [HttpPut("Update/Inventory")]
        public async Task<IActionResult> UpdateInventoryAsync(int id, [FromBody] UpdateInventoryDto dto)
        {
            var success = await _updateService.UpdateInventoryAsync(id, dto);
            if (!success)
                return NotFound("Inventory not found or update failed.");
            return Ok("Inventory updated successfully.");
        }
        #endregion

        #region VAT
        [HttpPut("VAT")]
        public async Task<IActionResult> UpdateVATAsync(int id, [FromBody] UpdateVATDto dto)
        {
            var success = await _updateService.UpdateVATAsync(id, dto);
            if (!success)
                return NotFound("VAT not found or update failed.");
            return Ok("VAT updated successfully.");
        }
        [HttpPut("ArchiveVat")]
        public async Task<ActionResult<ArchiveVatResponse>> ArchiveVatByIdAsync(int vatId)
        {
            if (vatId == 0)
                return BadRequest(new ArchiveVatResponse { Message = "VAT does not exist" });

            var archiveVat = await _updateService.ArchiveVATByIdAsync(vatId);
            if (!archiveVat)
                return BadRequest(new ArchiveVatResponse { Message = "Vat cannot be archived." });

            return Ok(new ArchiveVatResponse { Message = "VAT is now inactive." });
        }
        #endregion

        #region PurchaseOrder
        [HttpPut("PurchaseOrder")]
        public async Task<IActionResult> UpdatePurchaseOrderAsync(int id, [FromBody] UpdatePurchaseOrderDto dto)
        {
            var success = await _updateService.UpdatePurchaseOrderAsync(id, dto);
            if (!success)
                return NotFound("Purchase Order not found or update failed.");
            return Ok("Purchase Order updated successfully.");
        }
        [HttpPut("MarkAsReceived")]
        public async Task<ActionResult<bool>> MarkAsReceivedAsync(PurchaseOrderReceivedRequest request)
        {
            if (request is null)
                return BadRequest("Purchase order is empty");

            var purchaseOrder = await _updateService.MarkAsReceivedAsync(request);
            if (!purchaseOrder)
                return BadRequest("Please fill all the required properties.");

            return Ok(purchaseOrder);
        }
        #endregion
    }
}
