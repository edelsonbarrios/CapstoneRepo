using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.DataContext
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options) { }

        //AdminView
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        //CashierView
        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<SalesTransaction> SalesTransaction { get; set; }


        //InventoryManView
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

        //ItemDataControl
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Generic> Generic { get; set; }
        public DbSet<Formulation> Formulation { get; set; }
        public DbSet<Classification> Classification { get; set; }
        public DbSet<TypeModel> TypeModel { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Brand
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Generic
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Generic)
                .WithMany()
                .HasForeignKey(p => p.GenericID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Formulation
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Formulation)
                .WithMany()
                .HasForeignKey(p => p.FormulationID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Classification
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Classification)
                .WithMany()
                .HasForeignKey(p => p.ClassificationID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Type
            modelBuilder.Entity<Product>()
                .HasOne(p => p.TypeModel)
                .WithMany()
                .HasForeignKey(p => p.TypeID)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
