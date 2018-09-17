using System.Collections.Generic;
using GroceryStore2.Models;

namespace GroceryStore2.ViewModels
{
  public class ProductListViewModel
  {
    public IEnumerable<Product> Products { get; set; }
    public string CurrentCategory { get; set; }
  }
}