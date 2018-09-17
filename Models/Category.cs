using System.Collections.Generic;

namespace GroceryStore2.Models
{
    public class Category
    {
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public List<Product> Products { get; set; }
    }
}
