using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
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
        private readonly IGetServiceContracts _getService;

        public PutController(IUpdateServiceContracts updateService, IGetServiceContracts getService)
        {
            _updateService = updateService;
            _getService = getService;
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
        [HttpPut("Product")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto dto)
        {
            var success = await _updateService.UpdateProductAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }
        [HttpPut("ArchiveProduct")]
        public async Task<ActionResult<ArchiveProductResponse>> ArchiveProductByIdAsync(int productId)
        {
            if (productId == 0)
                return BadRequest(new ArchiveProductResponse { Message = "Product does not exist." });
            var archiveProduct = await _updateService.ArchiveProductByIdAsync(productId);
            if (!archiveProduct)
                return BadRequest(new ArchiveDiscountResponse { Message = "Product cannot be deleted." });
            return Ok(new ArchiveProductResponse { Message = "Product deleted successfully." });
        }
        #endregion

        #region Category
        [HttpPut("Category")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryDto dto)
        {
            var success = await _updateService.UpdateCategoryAsync(id, dto);
            if (!success)
                return NotFound("Category not found or update failed.");
            return Ok(new { Message = "Product sucessfully updated."});
        }
        [HttpPut("ArchiveCategory")]
        public async Task<ActionResult<object>> ArchiveCategoryByIdAsync(int categoryId)
        {
            if (categoryId == 0)
                return NotFound(new { Message = "Category does not exist." });
            var archiveCategory = await _updateService.ArchiveCategoryByIdAsync(categoryId);
            if (!archiveCategory)
                return BadRequest(new { Message = "Category cannot be updated." });
            return Ok(new { Message = "Category successsfully archived." });
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
        [HttpPut("ArchiveBrand")]
        public async Task<ActionResult<object>> ArchiveBrandByIdAsync(int brandId)
        {
            if (brandId == 0)
                return NotFound(new { Message = "Brand does not exist." });
            var updateBrand = await _updateService.ArchiveBrandByIdAsync(brandId);
            if (!updateBrand)
                return BadRequest(new { Message = "Brand cannot be deleted." });
            return Ok(new { Message = "Brand successfully deleted." });
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
        [HttpPut("ArchiveGeneric")]
        public async Task<ActionResult<object>> ArchiveGenericByIdAsync(int genericId)
        {
            if (genericId == 0)
                return NotFound(new { Message = "Generic does not exist." });
            var archiveGeneric = await _updateService.ArchiveGenericByIdAsync(genericId);
            if (!archiveGeneric)
                return BadRequest(new { Message = "Generic cannot be deleted." });
            return Ok(new { Message = "Generic successfully deleted." });
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
        [HttpPut("ArchiveFormulation")]
        public async Task<ActionResult<object>> ArchiveFormulationByIdAsync(int formulationId)
        {
            if (formulationId == 0)
                return NotFound(new { Message = "Formulation does not exist." });
            var archiveFormulation = await _updateService.ArchiveFormulationByIdAsync(formulationId);
            if (!archiveFormulation)
                return BadRequest(new { Message = "Formulation cannot be deleted." });
            return Ok(new { Message = "Formulation successfully deleted." });
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
        [HttpPut("ArchiveClassification")]
        public async Task<ActionResult<object>> ArchiveClassificationByIdAsync(int classificationId)
        {
            if (classificationId == 0)
                return NotFound(new { Message = "Classification does not exist." });
            var archiveClassification = await _updateService.ArchiveClassificationByIdAsync(classificationId);
            if (!archiveClassification)
                return BadRequest(new { Message = "Classification cannot be deleted." });
            return Ok(new { Message = "Classification successfully deleted." });
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
        [HttpPut("ArchiveType")]
        public async Task<ActionResult<object>> ArchiveTypeModelByIdAsync(int typeId)
        {
            if (typeId == 0)
                return NotFound(new { Message = "Type does not exist." });
            var archiveTypeModel = await _updateService.ArchiveTypeModelByIdAsync(typeId);
            if (!archiveTypeModel)
                return BadRequest(new { Message = "Type cannot be deleted." });
            return Ok(new { Message = "Type successfully deleted." });
        }
        #endregion

        #region Supplier
        [HttpPut("Supplier")]
        public async Task<IActionResult> UpdateSupplierAsync(int id, [FromBody] UpdateSupplierDto dto)
        {
            var success = await _updateService.UpdateSupplierAsync(id, dto);
            if (!success)
                return NotFound("Supplier not found or update failed.");
            return Ok(new {Message = "Supplier updated sucessfully."});
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
        [HttpPut("AssignProducts")]
        public async Task<ActionResult<AssignSupplierProductResponse>> AddProductsToSupplierAsync(AssignSupplierProductRequest request)
        {
            if (request.SupplierId == 0)
                return BadRequest(new AssignSupplierProductResponse { Message = "Supplier does not exist." });
            var assignProducts = await _updateService.AddProductsToSupplierAsync(request);
            if (!assignProducts)
                return BadRequest(new AssignSupplierProductResponse { Message = "Products cannot be added." });
            return Ok(new AssignSupplierProductResponse { Message = "Products added successfully." });
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

            foreach(var item in request.PurchaseOrderItems)
            {
                var product = await _getService.GetProductByIdAsync(item.ProductID);
                if (product.IsActive == false)
                    return NotFound("Product is inactive");
                if (item.BoxQuantityReceived <= 0)
                    return BadRequest($"Received Quantity of {product.ProductName} is 0. ");
            }

            var purchaseOrder = await _updateService.MarkAsReceivedAsync(request);
            if (!purchaseOrder)
                return BadRequest("Please fill all the required properties.");

            return Ok(purchaseOrder);
        }
        [HttpPut("ArchivePurchaseOrder")]
        public async Task<ActionResult<ArchivePurchaseOrderResponse>> ArchivePurchaseOrderByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest(new ArchivePurchaseOrderResponse { Message = "Purchase Order does not exist." });
            var archivePurchaseOrder = await _updateService.ArchivePurchaseOrderByIdAsync(id);
            if (!archivePurchaseOrder)
                return BadRequest(new ArchivePurchaseOrderResponse { Message = "Purchase Order cannot be deleted." });
            return Ok(archivePurchaseOrder);
        }
        #endregion
    }
}
