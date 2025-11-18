using Microsoft.EntityFrameworkCore;
using PharmacyBackend.DataContext;

namespace PharmacyBackend.Helper
{
  public static class CategoryHelper 
  {
    public static async Task<bool> CategoryRestorationValidation(
        PharmacyContext context,
        int categoryId
        ) 
    {
      var category = await context.Category
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.CategoryID == categoryId);

      if(category is null)
        return false;

      var duplication = await context.Category
        .AnyAsync(c => c.CategoryName.Equals(category.CategoryName) &&
            c.IsActive == true);

      if(duplication)
        return false;

      return true;
    }
  }
}
