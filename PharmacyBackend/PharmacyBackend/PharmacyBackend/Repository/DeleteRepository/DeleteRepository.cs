using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface;
using PharmacyBackend.DataContext;

namespace PharmacyBackend.Repository.DELETE
{
    public class DeleteRepository : IDeleteRepositoryContracts
    {
        private readonly PharmacyContext _context;
        public DeleteRepository(PharmacyContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
                return false;
            _context.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null)
                return false;
            _context.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        //sales
        public async Task<bool> DeleteSalesTransactionByIdAsync(int id)
        {
            var sale = await _context.SalesTransaction.FindAsync(id);
            if (sale == null)
                return false;

            _context.SalesTransaction.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        //Discount
        public async Task<bool> DeleteDiscountByIdAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) 
                return false;

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return true;
        }

        //Product
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) 
                return false;

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        //Category
        public async Task<bool> DeleteCategoryByIdAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) 
                return false;

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        //ItemDataControl

        //Brand
        public async Task<bool> DeleteBrandByIdAsync(int id)
        {
            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
                return false;

            _context.Brand.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        //Generic
        public async Task<bool> DeleteGenericByIdAsync(int id)
        {
            var generic = await _context.Generic.FindAsync(id);
            if (generic == null)
                return false;

            _context.Generic.Remove(generic);
            await _context.SaveChangesAsync();
            return true;
        }

        //Formulation
        public async Task<bool> DeleteFormulationByIdAsync(int id)
        {
            var formulation = await _context.Formulation.FindAsync(id);
            if (formulation == null)
                return false;

            _context.Formulation.Remove(formulation);
            await _context.SaveChangesAsync();
            return true;
        }

        //Classififcation
        public async Task<bool> DeleteClassificationByIdAsync(int id)
        {
            var classification = await _context.Classification.FindAsync(id);
            if (classification == null)
                return false;

            _context.Classification.Remove(classification);
            await _context.SaveChangesAsync();
            return true;
        }

        //Type
        public async Task<bool> DeleteTypeModelByIdAsync(int id)
        {
            var typemodel= await _context.TypeModel.FindAsync(id);
            if (typemodel == null)
                return false;

            _context.TypeModel.Remove(typemodel);
            await _context.SaveChangesAsync();
            return true;
        }

        //Supplier/
        public async Task<bool> DeleteSupplierByIdAsync(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
                return false;

            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        //Inventory
        public async Task<bool> DeleteInventoryByIdAsync(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
                return false;

            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        //VAT
        public async Task<bool> DeleteVATByIdAsync(int id)
        {
            var vat = await _context.VAT.FindAsync(id);
            if (vat == null)
                return false;

            _context.VAT.Remove(vat);
            await _context.SaveChangesAsync();
            return true;
        }

        //PurchaseOrder
        public async Task<bool> DeletePurchaseOrderByIdAsync(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
            if (purchaseOrder == null)
                return false;

            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}