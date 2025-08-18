using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.Repository.DELETE;

namespace PharmacyBackend.Service.DeleteService
{
    public class DeleteService : IDeleteServiceContracts
    {
        private readonly IDeleteRepositoryContracts _deleteRepo;

        public DeleteService(IDeleteRepositoryContracts deleteRepo)
        {
            _deleteRepo = deleteRepo;
        }

        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var deleteAccount = await _deleteRepo.DeleteAccountByIdAsync(id);
            if (!deleteAccount)
                return false;
            return true;
        }

        //employee
        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            var deleteEmployee = await _deleteRepo.DeleteEmployeeByIdAsync(id);
            if (!deleteEmployee)
                return false;
            return true;
        }
        //sales
        public async Task<bool> DeleteSalesTransactionByIdAsync(int id)
        {
            var deletedSale = await _deleteRepo.DeleteSalesTransactionByIdAsync(id);
            if (!deletedSale)
                return false;
            return true;
        }

        //Discount
        public async Task<bool> DeleteDiscountByIdAsync(int id)
        {
            var deletedDiscount = await _deleteRepo.DeleteDiscountByIdAsync(id);
            if (!deletedDiscount)
                return false;
            return true;
        }

        //Product
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var deleteProduct = await _deleteRepo.DeleteProductByIdAsync(id);
            if(!deleteProduct)
                return false;
            return true;
        }

        //Category
        public async Task<bool> DeleteCategoryByIdAsync(int id)
        {
            var deleteCategory = await _deleteRepo.DeleteCategoryByIdAsync(id);
            if (!deleteCategory)
                return false;
            return true;
        }

        //ItemDataControl

        //Brand
        public async Task<bool> DeleteBrandByIdAsync(int id)
        {
            var deleteBrand = await _deleteRepo.DeleteBrandByIdAsync(id);
            if (!deleteBrand)
                return false;
            return true;
        }

        //Generic
        public async Task<bool> DeleteGenericByIdAsync(int id)
        {
            var deleteGeneric = await _deleteRepo.DeleteGenericByIdAsync(id);
            if (!deleteGeneric)
                return false;
            return true;
        }

        //Formulation
        public async Task<bool> DeleteFormulationByIdAsync(int id)
        {
            var deleteFormulation = await _deleteRepo.DeleteFormulationByIdAsync(id);
            if (!deleteFormulation)
                return false;
            return true;
        }

        //Classification
        public async Task<bool> DeleteClassificationByIdAsync(int id)
        {
            var deleteClassification = await _deleteRepo.DeleteClassificationByIdAsync(id);
            if (!deleteClassification)
                return false;
            return true;
        }

        //Type
        public async Task<bool> DeleteTypeModelByIdAsync(int id)
        {
            var deleteType = await _deleteRepo.DeleteTypeModelByIdAsync(id);
            if (!deleteType)
                return false;
            return true;
        }

        //Supplier
        public async Task<bool> DeleteSupplierByIdAsync(int id)
        {
            var deleteSupplier = await _deleteRepo.DeleteSupplierByIdAsync(id);
            if (!deleteSupplier)
                return false;
            return true;
        }

        //Inventory
        public async Task<bool> DeleteInventoryByIdAsync(int id)
        {
            var deleteInventory = await _deleteRepo.DeleteInventoryByIdAsync(id);
            if (!deleteInventory)
                return false;
            return true;
        }

        //VAT
        public async Task<bool> DeleteVATByIdAsync(int id)
        {
            var deleteVAT = await _deleteRepo.DeleteVATByIdAsync(id);
            if (!deleteVAT)
                return false;
            return true;
        }

        //PurchaseOrder
        public async Task<bool> DeletePurchaseOrderByIdAsync(int id)
        {
            var deletePurchaseOrder = await _deleteRepo.DeletePurchaseOrderByIdAsync(id);
            if (!deletePurchaseOrder)
                return false;
            return true;
        }

    }
}
