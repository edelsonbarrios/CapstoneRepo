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

        public async Task<bool> UpdateAccountAsync(Account request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }

        //employee
        public async Task<bool> UpdateEmployeeAsync(Employee request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }

        //sales
        public async Task<bool> UpdateSalesTransactionAsync(SalesTransaction sale)
        {
            _context.SalesTransaction.Update(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        //Discount
        public async Task<bool> UpdateDiscountAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return true;
        }

        //Product
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        //Category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        //ItemDataControl

        //Brand
        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            _context.Brand.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        //Generic
        public async Task<bool> UpdateGenericAsync(Generic generic)
        {
            _context.Generic.Update(generic);
            await _context.SaveChangesAsync();
            return true;
        }

        //Formulation
        public async Task<bool> UpdateFormulationAsync(Formulation formulation)
        {
            _context.Formulation.Update(formulation);
            await _context.SaveChangesAsync();
            return true;
        }

        //Classification
        public async Task<bool> UpdateClassificationAsync(Classification classification)
        {
            _context.Classification.Update(classification);
            await _context.SaveChangesAsync();
            return true;
        }

        //TypeModel
        public async Task<bool> UpdateTypeModelAsync(TypeModel typeModel)
        {
            _context.TypeModel.Update(typeModel);
            await _context.SaveChangesAsync();
            return true;
        }

        //Supplier
        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            _context.Supplier.Update(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        //Inventory
        public async Task<bool> UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventory.Update(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        //VAT
        public async Task<bool> UpdateVATAsync(VAT vat)
        {
            _context.VAT.Update(vat);
            await _context.SaveChangesAsync();
            return true;
        }

        //PurchaseOrder
        public async Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrder.Update(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
