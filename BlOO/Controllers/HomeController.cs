using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlOO.Models;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlOO.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private UserManager<ApplicationUser> _UserManager;
    private RoleManager<IdentityRole<int>> _RoleManager;
    private SignInManager<ApplicationUser> _SignInManager;
    public HomeController(ILogger<HomeController> logger , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _UserManager = userManager;
        _RoleManager = roleManager;
        _SignInManager = signInManager;
    }



    public IActionResult Welcome()
    {
        ViewData["HideNavbar"] = "true";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
