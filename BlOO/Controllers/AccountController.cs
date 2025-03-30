using BlOO.Models;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BlOO.Controllers
{
    public class AccountController : Controller
    {
        //public UserManager<ApplicationUser> userManager;
        public AccountController()//UserManager<ApplicationUser> userManager)
        {
                //this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterData(RegisterViewModel registerViewModel)
        {
            //if (ModelState.IsValid)
            //{
            //    ApplicationUser AppUser = new ApplicationUser();
            //    AppUser.UserName = registerViewModel.UserName;
            //    AppUser.PasswordHash = registerViewModel.Password;
            //    AppUser.Email = registerViewModel.Email;

            //    userManager.CreateAsync

            //}
            return View("Register", registerViewModel);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View("LogIn");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogInData(LoginUserViewModel loginUserViewModel)
        {
            //if (ModelState.IsValid)
            //{

            //}
            return View("LogIn", loginUserViewModel);
        }
    }
}
