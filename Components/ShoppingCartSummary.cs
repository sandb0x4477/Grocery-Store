using GroceryStore2.Models;
using GroceryStore2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore2.Components
{
  public class ShoppingCartSummary : ViewComponent
  {
    private readonly ShoppingCart _shoppingCart;

    public ShoppingCartSummary(ShoppingCart shoppingCart)
    {
      _shoppingCart = shoppingCart;
    }

    public IViewComponentResult Invoke()
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

  }
}
