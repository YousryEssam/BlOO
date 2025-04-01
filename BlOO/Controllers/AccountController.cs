using BlOO.Models;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Threading.Tasks;

namespace BlOO.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        public RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager; 
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterData(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser existingUser = await userManager.FindByEmailAsync(registerViewModel.Email);
                if (existingUser == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.FirstName = registerViewModel.FirstName;
                    user.LastName = registerViewModel.LastName;
                    user.Email = registerViewModel.Email;

                    if (existingUser.Email == "Admin@gmail.com")
                    {
                        await AddRole();
                        await userManager.AddToRoleAsync(user, "Admin");

                    }

                    IdentityResult result = await userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("HomePage","Post");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                else
                {
                    ModelState.AddModelError("Email", "Email is already exist");

                }


            }
            return View("Register", registerViewModel);
        }


        [NonAction]
        public async Task<bool> AddRole()
        {
            IdentityRole role = new IdentityRole();
            role.Name = "Admin";
            IdentityResult result= await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return true;//ملهمش لزمة لحد دلوقتي
            }
            return false;//already exist + ملهمش لزمة لحد دلوقتي

        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
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
