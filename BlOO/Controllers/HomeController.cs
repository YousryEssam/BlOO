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
    private readonly IPostRepository _postRepository;

    public HomeController(ILogger<HomeController> logger , UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager,
        IApplicationUserRepository applicationUserRepository, IPostRepository postRepository)
    {
        _logger = logger;
        _UserManager = userManager;
        _RoleManager = roleManager;
        _SignInManager = signInManager;
        _applicationUserRepository = applicationUserRepository;
        _postRepository = postRepository;
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

    public IActionResult ShowUsersOrPosts(string searchvalue)
    {
        if (!string.IsNullOrEmpty(searchvalue))
        {
            List<ApplicationUser> applicationUsers = _applicationUserRepository.SearchByName(searchvalue);
            List<Post> posts = _postRepository.SearchByName(searchvalue);
            return View((applicationUsers, posts));
        }
        return View("Welcome");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
