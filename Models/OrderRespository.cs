using System;

namespace GroceryStore2.Models
{
  public class OrderRespository : IOrderRepository
  {
    private readonly AppDbContext _appDbContext;
    private readonly ShoppingCart _shoppingCart;

    public OrderRespository(AppDbContext appDbContext, ShoppingCart shoppingCart)
    {
      _appDbContext = appDbContext;
      _shoppingCart = shoppingCart;
    }

    public void CreateOrder(Order order)
    {
      order.OrderPlaced = DateTime.Now;
      order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

      _appDbContext.Orders.Add(order);

      var shoppingCartItems = _shoppingCart.ShoppingCartItems;

      foreach (var shoppingCartItem in shoppingCartItems)
      {
        var orderDetail = new OrderDetail()
        {
          Amount = shoppingCartItem.Amount,
          ProductId = shoppingCartItem.Product.ProductId,
          OrderId = order.OrderId,
          Price = shoppingCartItem.Product.Price
        };

        _appDbContext.OrderDetails.Add(orderDetail);
      }

      _appDbContext.SaveChanges();
    }
  }
}
