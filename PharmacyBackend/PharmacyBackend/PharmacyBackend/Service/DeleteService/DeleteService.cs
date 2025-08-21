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

        #region Account
        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var deleteAccount = await _deleteRepo.DeleteAccountByIdAsync(id);
            if (!deleteAccount)
                return false;
            return true;
        }
        #endregion

        #region Employee
        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            var deleteEmployee = await _deleteRepo.DeleteEmployeeByIdAsync(id);
            if (!deleteEmployee)
                return false;
            return true;
        }
        #endregion

        #region Sales
        public async Task<bool> DeleteSalesTransactionByIdAsync(int id)
        {
            var deletedSale = await _deleteRepo.DeleteSalesTransactionByIdAsync(id);
            if (!deletedSale)
                return false;
            return true;
        }
        #endregion

        #region Discount
        public async Task<bool> DeleteDiscountByIdAsync(int id)
        {
            var deletedDiscount = await _deleteRepo.DeleteDiscountByIdAsync(id);
            if (!deletedDiscount)
                return false;
            return true;
        }
        #endregion

        #region Product
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var deleteProduct = await _deleteRepo.DeleteProductByIdAsync(id);
            if (!deleteProduct)
                return false;
            return true;
        }
        #endregion

        #region Category
        public async Task<bool> DeleteCategoryByIdAsync(int id)
        {
            var deleteCategory = await _deleteRepo.DeleteCategoryByIdAsync(id);
            if (!deleteCategory)
                return false;
            return true;
        }
        #endregion

        #region Brand
        public async Task<bool> DeleteBrandByIdAsync(int id)
        {
            var deleteBrand = await _deleteRepo.DeleteBrandByIdAsync(id);
            if (!deleteBrand)
                return false;
            return true;
        }
        #endregion

        #region Generic
        public async Task<bool> DeleteGenericByIdAsync(int id)
        {
            var deleteGeneric = await _deleteRepo.DeleteGenericByIdAsync(id);
            if (!deleteGeneric)
                return false;
            return true;
        }
        #endregion

        #region Formulation
        public async Task<bool> DeleteFormulationByIdAsync(int id)
        {
            var deleteFormulation = await _deleteRepo.DeleteFormulationByIdAsync(id);
            if (!deleteFormulation)
                return false;
            return true;
        }
        #endregion

        #region Classification
        public async Task<bool> DeleteClassificationByIdAsync(int id)
        {
            var deleteClassification = await _deleteRepo.DeleteClassificationByIdAsync(id);
            if (!deleteClassification)
                return false;
            return true;
        }
        #endregion

        #region Type
        public async Task<bool> DeleteTypeModelByIdAsync(int id)
        {
            var deleteType = await _deleteRepo.DeleteTypeModelByIdAsync(id);
            if (!deleteType)
                return false;
            return true;
        }
        #endregion

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
