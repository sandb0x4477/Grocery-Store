using System.Collections.Generic;

namespace GroceryStore2.Models
{
    public interface ICategoryRepository
    {
      IEnumerable<Category> GetCategories();
    }
}
