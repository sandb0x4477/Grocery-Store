using System.Collections.Generic;
using System.Linq;

namespace GroceryStore2.Models
{
    public class CategoryRepository : ICategoryRepository
    {
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
      {
        _appDbContext = appDbContext;
      }

    public IEnumerable<Category> GetCategories()
      {
        return _appDbContext.Categories.OrderBy(c => c.CategoryName);
      }
    }
}
