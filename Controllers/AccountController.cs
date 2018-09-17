using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GroceryStore2.Models;
using GroceryStore2.ViewModels;
using System.Threading.Tasks;

namespace GroceryStore2.Controllers
{

  [Authorize]
  public class AccountController : Controller
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
      return View(new LoginViewModel
      {
        ReturnUrl = returnUrl
      });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
      if (!ModelState.IsValid)
        return View(loginViewModel);

      var user = await _userManager.FindByNameAsync(loginViewModel.Email);

      if (user != null)
      {
        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false);

        var isSigned = _signInManager.IsSignedIn(User);

        if (result.Succeeded)
        {
          if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
          {
            return RedirectToAction("Index", "Home");
          }

          return Redirect(loginViewModel.ReturnUrl);
        }
      }

      ModelState.AddModelError("", "Email/password not found");
      return View(loginViewModel);
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(LoginViewModel loginViewModel)
    {
      if (ModelState.IsValid)
      {
        var user = new AppUser { UserName = loginViewModel.Email, Email = loginViewModel.Email };
        var result = await _userManager.CreateAsync(user, loginViewModel.Password);

        if (result.Succeeded)
        {
          return RedirectToAction("Login", "Account");
        }
      }
      return View(loginViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}
