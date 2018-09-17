using System.Collections.Generic;
using GroceryStore2.Models;

namespace GroceryStore2.ViewModels
{
  public class HomeViewModel
  {
    public IEnumerable<Product> FeaturedProducts { get; set; }
  }
}