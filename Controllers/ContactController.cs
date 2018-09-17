using Microsoft.AspNetCore.Mvc;

namespace GroceryStore2.Controllers
{
  public class ContactController : Controller
  {
    // GET
    public IActionResult Index()
    {
      return
      View();
    }
  }
}