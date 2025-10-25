using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts;
using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.Repository.DELETE;

namespace PharmacyBackend.Service.DeleteService
{
    public class DeleteService : IDeleteServiceContracts
    {
        private readonly IDeleteRepositoryContracts _deleteRepo;
        private readonly IConfiguration _config;

        public DeleteService(IDeleteRepositoryContracts deleteRepo, IConfiguration config)
        {
            _deleteRepo = deleteRepo;
            _config = config;
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

        public async Task<bool> DeleteSalesItemAsync(int id)
        {
            var deletedSalesItem = await _deleteRepo.DeleteSalesItemAsync(id);
            if (!deletedSalesItem)
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

        #region Supplier
        public async Task<bool> DeleteSupplierByIdAsync(int id)
        {
            var deleteSupplier = await _deleteRepo.DeleteSupplierByIdAsync(id);
            if (!deleteSupplier)
                return false;
            return true;
        }
        #endregion

        #region Inventory
        public async Task<bool> DeleteInventoryByIdAsync(int id)
        {
            var deleteInventory = await _deleteRepo.DeleteInventoryByIdAsync(id);
            if (!deleteInventory)
                return false;
            return true;
        }
        #endregion

        #region VAT
        public async Task<bool> DeleteVATByIdAsync(int id)
        {
            var deleteVAT = await _deleteRepo.DeleteVATByIdAsync(id);
            if (!deleteVAT)
                return false;
            return true;
        }
        #endregion

        #region PurchaseOrder
        public async Task<bool> DeletePurchaseOrderByIdAsync(int id)
        {
            var deletePurchaseOrder = await _deleteRepo.DeletePurchaseOrderByIdAsync(id);
            if (!deletePurchaseOrder)
                return false;
            return true;
        }
        #endregion

        #region BackUp Database
        public async Task<DeleteBackupResponse> DeleteBackupAsync(string fileName)
        {
            try
            {
                var backupDir = GetBackupDirectory();
                var filePath = Path.Combine(backupDir, fileName);
                if (!File.Exists(filePath))
                {
                    return new DeleteBackupResponse
                    {
                        Success = false,
                        Message = "Backup file not found"
                    };
                }
                File.Delete(filePath);
                return new DeleteBackupResponse
                {
                    Success = true,
                    Message = "Backup deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteBackupResponse
                {
                    Success = false,
                    Message = $"Failed to delete backup: {ex.Message}"
                };
            }
        }

        // Interface method - no parameters, reads from config
        public async Task CleanupOldBackupsAsync()
        {
            // Read values from configuration
            var retentionDays = _config.GetValue<int>("BackupSettings:RetentionDays", 30);
            var maxBackupCount = _config.GetValue<int>("BackupSettings:MaxBackupCount", 50);

            await CleanupOldBackupsAsync(retentionDays, maxBackupCount);
        }

        // Helper method - keeps your existing logic
        private async Task<bool> CleanupOldBackupsAsync(int retentionDays, int maxBackupCount)
        {
            try
            {
                var backupDir = GetBackupDirectory();
                if (!Directory.Exists(backupDir))
                {
                    return true;
                }
                var backupFiles = Directory.GetFiles(backupDir, "*.backup")
                    .Select(file => new FileInfo(file))
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();
                var cutoffDate = DateTime.Now.AddDays(-retentionDays);
                var filesToDelete = new List<FileInfo>();
                // Delete files older than retention period
                filesToDelete.AddRange(backupFiles.Where(f => f.CreationTime < cutoffDate));
                // If still exceeding max count, delete oldest files
                if (backupFiles.Count > maxBackupCount)
                {
                    var excessFiles = backupFiles
                        .Skip(maxBackupCount)
                        .Where(f => !filesToDelete.Contains(f));
                    filesToDelete.AddRange(excessFiles);
                }
                foreach (var file in filesToDelete.Distinct())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        // Log but continue with other files
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetBackupDirectory()
        {
            var backupPath = _config.GetValue<string>("BackupSettings:BackupPath");
            if (string.IsNullOrEmpty(backupPath))
            {
                backupPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            }
            return backupPath;
        }
        #endregion

    }
}
