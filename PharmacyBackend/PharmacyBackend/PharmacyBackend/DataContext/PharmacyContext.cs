using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.DataContext
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options) { }

        #region Tables
        //AdminView
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistory { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }

        // Product Management
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Generic> Generic { get; set; }
        public DbSet<Formulation> Formulation { get; set; }
        public DbSet<Classification> Classification { get; set; }
        public DbSet<TypeModel> TypeModel { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        // Inventory Management
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }
        public DbSet<StockMovement> StockMovement { get; set; }

        // Sales Management
        public DbSet<SalesItem> SalesItem { get; set; }
        public DbSet<SalesTransaction> SalesTransaction { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        // Tax Management
        public DbSet<VAT> VAT { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee-Account-Role relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeHistory>()
                .HasOne(eh => eh.Employee)
                .WithMany()
                .HasForeignKey(eh => eh.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeHistory>()
                .HasOne(eh => eh.PerformedByEmployee)
                .WithMany()
                .HasForeignKey(eh => eh.PerformedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Product relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Generic)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.GenericID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Formulation)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FormulationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Classification)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ClassificationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.TypeModel)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.TypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Suppliers)
                .WithMany(s => s.Products);

            // Inventory relationships
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Inventory)
                .HasForeignKey(i => i.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseOrder relationships
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Employee)
                .WithMany(e => e.PurchaseOrders)
                .HasForeignKey(po => po.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrderItem relationships
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderItems)
                .HasForeignKey(poi => poi.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.Product)
                .WithMany(p => p.PurchaseOrderItem)
                .HasForeignKey(poi => poi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // StockMovement relationships
            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Employee)
                .WithMany(e => e.StockMovements)
                .HasForeignKey(sm => sm.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Inventory)
                .WithMany(i => i.StockMovements)
                .HasForeignKey(sm => sm.InventoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // SalesTransaction relationships
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.Employee)
                .WithMany(e => e.SalesTransactions)
                .HasForeignKey(st => st.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.Discount)
                .WithMany()
                .HasForeignKey(st => st.DiscountId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalesItem relationships
            modelBuilder.Entity<SalesItem>()
                .HasOne(si => si.SalesTransaction)
                .WithMany(st => st.Items)
                .HasForeignKey(si => si.SalesTransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalesItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.SalesItem)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision for monetary values
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<VAT>()
                .Property(v => v.VATRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Inventory>()
                .Property(i => i.CostPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Inventory>()
                .Property(i => i.SellingPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(poi => poi.UnitCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(poi => poi.TotalCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesItem>()
                .Property(si => si.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesItem>()
                .Property(si => si.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTransaction>()
                .Property(st => st.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTransaction>()
                .Property(st => st.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTransaction>()
                .Property(st => st.NetAmount)
                .HasPrecision(18, 2);

            // Configure unique constraints
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(s => s.SupplierName)
                .IsUnique();

            modelBuilder.Entity<VAT>()
                .HasIndex(v => v.VATName)
                .IsUnique();

            // Configure default values
            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.Status)
                .HasDefaultValue("Pending");

            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<StockMovement>()
                .Property(sm => sm.MovementDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<SalesTransaction>()
                .Property(st => st.TransactionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure indexes for better performance
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductName);

            modelBuilder.Entity<Inventory>()
                .HasIndex(i => new { i.ProductID, i.BatchNumber });

            modelBuilder.Entity<Inventory>()
                .HasIndex(i => i.ExpirationDate);

            modelBuilder.Entity<StockMovement>()
                .HasIndex(sm => sm.MovementDate);

            modelBuilder.Entity<SalesTransaction>()
                .HasIndex(st => st.TransactionDate);

            modelBuilder.Entity<SalesItem>().ToTable("SalesItem");

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    SupplierName = "Default Supplier",
                    ContactNumber = "",
                    Email = "",
                    Address = "",
                    IsActive = true
                }
            );

        }
    }
}
