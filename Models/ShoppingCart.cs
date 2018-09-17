using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStore2.Models
{
  public class ShoppingCart
  {
    private readonly AppDbContext _appDbContext;

    private ShoppingCart(AppDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }

    public string ShoppingCartId { get; set; }

    public List<ShoppingCartItem> ShoppingCartItems { get; set; }

    public static ShoppingCart GetCart(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?
          .HttpContext.Session;

      var context = services.GetService<AppDbContext>();
      string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

      session.SetString("CartId", cartId);

      return new ShoppingCart(context) { ShoppingCartId = cartId };
    }

    public void AddToCart(Product product, int amount = 1)
    {
      var shoppingCartItem =
              _appDbContext.ShoppingCartItems.SingleOrDefault(
                  s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId);

      if (shoppingCartItem == null)
      {
        shoppingCartItem = new ShoppingCartItem
        {
          ShoppingCartId = ShoppingCartId,
          Product = product,
          Amount = amount
        };

        _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
      }
      else
      {
        shoppingCartItem.Amount++;
      }
      _appDbContext.SaveChanges();
    }

    public int RemoveFromCart(Product product)
    {
      var shoppingCartItem =
              _appDbContext.ShoppingCartItems.SingleOrDefault(
                  s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId);

      var localAmount = 0;

      if (shoppingCartItem != null)
      {
        if (shoppingCartItem.Amount > 1)
        {
          shoppingCartItem.Amount--;
          localAmount = shoppingCartItem.Amount;
        }
        else
        {
          _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
        }
      }

      _appDbContext.SaveChanges();

      return localAmount;
    }

    public void ClearCart()
    {
      var cartItems = _appDbContext
          .ShoppingCartItems
          .Where(cart => cart.ShoppingCartId == ShoppingCartId);

      _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

      _appDbContext.SaveChanges();
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
      return ShoppingCartItems ??
             (ShoppingCartItems =
                 _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                     .Include(s => s.Product)
                     .ToList());
    }

    public decimal GetShoppingCartTotal()
    {
      var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
          .Select(c => c.Product.Price * c.Amount).Sum();
      return total;
    }
  }
}
