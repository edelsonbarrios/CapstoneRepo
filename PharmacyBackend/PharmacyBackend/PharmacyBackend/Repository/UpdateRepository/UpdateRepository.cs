using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Repository.UpdateRepository
{
    public class UpdateRepository : IUpdateRepositoryContracts
    {
        private readonly PharmacyContext _context;
        public UpdateRepository(PharmacyContext context)
        {
            _context = context;
        }
        #region Account
        public async Task<bool> UpdateAccountAsync(Account request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Employee
        public async Task<bool> UpdateEmployeeAsync(Employee request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Sales
        public async Task<bool> UpdateSalesTransactionAsync(SalesTransaction sale)
        {
            _context.SalesTransaction.Update(sale);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Discount
        public async Task<bool> UpdateDiscountAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Product
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Brand
        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            _context.Brand.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Generic
        public async Task<bool> UpdateGenericAsync(Generic generic)
        {
            _context.Generic.Update(generic);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Formulation
        public async Task<bool> UpdateFormulationAsync(Formulation formulation)
        {
            _context.Formulation.Update(formulation);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Classification
        public async Task<bool> UpdateClassificationAsync(Classification classification)
        {
            _context.Classification.Update(classification);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Type
        public async Task<bool> UpdateTypeModelAsync(TypeModel typeModel)
        {
            _context.TypeModel.Update(typeModel);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Supplier
        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            _context.Supplier.Update(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Inventory
        public async Task<bool> UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventory.Update(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region VAT
        public async Task<bool> UpdateVATAsync(VAT vat)
        {
            _context.VAT.Update(vat);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region PurchaseOrder
        public async Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrder.Update(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
