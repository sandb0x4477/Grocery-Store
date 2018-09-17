using GroceryStore2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GroceryStore2.ViewModels;

namespace GroceryStore2.Controllers
{
    public class ProductController : Controller
    {
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
          _productRepository = productRepository;
          _categoryRepository = categoryRepository;
        }

    public ViewResult List(string category)
    {
      IEnumerable<Product> products;
      string currentCategory;

      if (string.IsNullOrEmpty(category))
      {
        products = _productRepository.Products.OrderBy(p => p.Name);
        currentCategory = "All";
      }
      else
      {
        {
          products = _productRepository.Products.Where(p => p.Category.CategoryName == category)
            .OrderBy(p => p.Name);
          currentCategory = _categoryRepository.GetCategories()
            .FirstOrDefault(c => c.CategoryName == category)
            ?.CategoryName;
        }
      }

      return View(new ProductListViewModel
      {
        Products = products,
        CurrentCategory = currentCategory
        });
    }
  }
}
