using BlOO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlOO.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        public RoleManager<IdentityRole<int>> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            ViewData["HideNavbar"] = "true";
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
                    user.UserName = registerViewModel.Email;


                    IdentityResult result = await userManager.CreateAsync(user, registerViewModel.Password);


                    if (user.Email == "Admin@gmail.com")
                    {
                        await MakeRole();
                        await userManager.AddToRoleAsync(user, "Admin");

                    }
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("LogIn", "Account");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
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
        public async Task<bool> MakeRole()
        {
            IdentityRole<int> role = new IdentityRole<int>();
            role.Name = "Admin";
            IdentityResult result = await roleManager.CreateAsync(role);
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
            ViewData["HideNavbar"] = "true";
            return View("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInData(LoginUserViewModel UserFromLogin)
        {

            if (!ModelState.IsValid)
            {
                return View("LogIn", UserFromLogin);
            }

            ApplicationUser UserFromDatabase = await userManager.FindByEmailAsync(UserFromLogin.Email);
            if (UserFromDatabase == null)
            {
                return ReturnInvalidLogin(UserFromLogin);
            }


            bool CorrectPassworded = await userManager.CheckPasswordAsync(UserFromDatabase, UserFromLogin.Password);
            if (!CorrectPassworded)
            {
                return ReturnInvalidLogin(UserFromLogin);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("imgUrl", UserFromDatabase.ProfileImageUrl ?? ""),
                new Claim("FirstName", UserFromDatabase.FirstName ?? ""),
                new Claim("LastName", UserFromDatabase.LastName ?? "")
            };

            await signInManager.SignInWithClaimsAsync(UserFromDatabase, UserFromLogin.RememberMe, claims);

            if(UserFromDatabase.Email == "Admin@gmail.com")
            {
                return RedirectToAction("AdminPage", "Admin");
            }

            return RedirectToAction("HomePage", "Post", new { id = UserFromDatabase.Id });
        }


        ///////////////////////////////// Helper Methods /////////////////////////////////////////

        // Helper Method for Invalid Logins
        private IActionResult ReturnInvalidLogin(LoginUserViewModel UserFromLogin)
        {
            ModelState.AddModelError("", "Incorrect Login attempt");
            return View("LogIn", UserFromLogin);
        }
    }
}