namespace PharmacyBackend.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Product> Product { get; set; }
    }
}
