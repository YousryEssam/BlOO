using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlOO.Models;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Identity;
using BlOO.Repositories;

namespace BlOO.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private UserManager<ApplicationUser> _UserManager;
    private RoleManager<IdentityRole<int>> _RoleManager;
    private SignInManager<ApplicationUser> _SignInManager;
    private IApplicationUserRepository _applicationUserRepository;
    public HomeController(ILogger<HomeController> logger , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager,IApplicationUserRepository applicationUserRepository)
    {
        _logger = logger;
        _UserManager = userManager;
        _RoleManager = roleManager;
        _SignInManager = signInManager;
        _applicationUserRepository = applicationUserRepository;
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

    public IActionResult ShowUsers(string searchvalue)
    {
        if (!string.IsNullOrEmpty(searchvalue))
        {
            List<ApplicationUser> applicationUsers = _applicationUserRepository.SearchByName(searchvalue);
            return View(applicationUsers);
        }
        return View("Welcome");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
