using System.Linq;
using GroceryStore2.Models;
using GroceryStore2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore2.Controllers
{
  public class ShoppingCartController : Controller
  {
    private readonly IProductRepository _productRepository;
    private readonly ShoppingCart _shoppingCart;

    public ShoppingCartController(IProductRepository productRepository, ShoppingCart shoppingCart)
    {
      _productRepository = productRepository;
      _shoppingCart = shoppingCart;
    }
    public ViewResult Index()
    {
      var items = _shoppingCart.GetShoppingCartItems();
      _shoppingCart.ShoppingCartItems = items;

      var shoppingCartViewModel = new ShoppingCartViewModel
      {
        ShoppingCart = _shoppingCart,
        ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
      };

      return View(shoppingCartViewModel);
    }

    public RedirectToActionResult AddToShoppingCart(int productId)
    {
      var selectedProduct = _productRepository.Products.FirstOrDefault(p => p.ProductId == productId);

      if (selectedProduct != null)
      {
        _shoppingCart.AddToCart(selectedProduct, 1);
      }
      return RedirectToAction("Index");
    }

    public RedirectToActionResult ClearShoppingCart()
    {
      _shoppingCart.ClearCart();

      return RedirectToAction("Index");
    }
  }
}
