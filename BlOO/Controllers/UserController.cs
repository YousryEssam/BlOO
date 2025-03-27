using BlOO.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class UserController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Profile(int id)
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();

            return View(profileViewModel);
        }
    }
}
