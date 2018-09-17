using GroceryStore2.Models;
using GroceryStore2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore2.Controllers
{
    public class HomeController : Controller
    {
      private readonly IProductRepository _productRepository;

      public HomeController(IProductRepository productRepository)
      {
        _productRepository = productRepository;
      }

      public ViewResult Index()
      {
        var homeViewModel = new HomeViewModel
        {
          FeaturedProducts = _productRepository.FeaturedProducts
        };

        return View(homeViewModel);
      }
    }
}
