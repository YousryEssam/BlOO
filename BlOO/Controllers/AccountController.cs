using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlOO.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        public RoleManager<IdentityRole<int>> roleManager;
        private IApplicationUserRepository _userRepository;
        public SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<int>> roleManager ,IApplicationUserRepository applicationUserRepository )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _userRepository = applicationUserRepository;
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
                return true;
            }
            return false;

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


        [Authorize]
        public async Task<IActionResult> UpdateAccess()
        {
            var userFromDb = await userManager.GetUserAsync(User);
            if (userFromDb == null)
            {
                return NotFound();
            }

            UpdateAccessViewModel accountInfo = new UpdateAccessViewModel
            {
                ProfileId = userFromDb.Id,
                Email = userFromDb.Email
            };

            return View(accountInfo);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAccountAccessUpdates(UpdateAccessViewModel accountAccessFromView)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateAccess", accountAccessFromView);
            }

            var accountFromDB = await userManager.FindByIdAsync(accountAccessFromView.ProfileId.ToString());
            
            if (accountFromDB == null)
            {
                return NotFound();
            }

            // Validate password first
            var isCorrectPassword = await userManager.CheckPasswordAsync(accountFromDB, accountAccessFromView.OldPassword);

            if (!isCorrectPassword)
            {
                ModelState.AddModelError("OldPassword", "The password is incorrect.");
                return View("UpdateAccess", accountAccessFromView);
            }

            // Handle email update
            if (accountFromDB.NormalizedEmail != accountAccessFromView.Email.ToUpper())
            {
                if (!_userRepository.IsAvailableEmail(accountAccessFromView.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View("UpdateAccess", accountAccessFromView);
                }

                accountFromDB.Email = accountAccessFromView.Email;
                var emailUpdateResult = await userManager.UpdateAsync(accountFromDB);
                if (!emailUpdateResult.Succeeded)
                {
                    foreach (var error in emailUpdateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("UpdateAccess", accountAccessFromView);
                }
            }

            // Handle password update if provided
            bool wantsToChangePassword = !string.IsNullOrEmpty(accountAccessFromView.NewPassword) ||
                                        !string.IsNullOrEmpty(accountAccessFromView.ConfirmPassword);

            if (wantsToChangePassword)
            {
                // Make sure both fields are provided
                if (string.IsNullOrEmpty(accountAccessFromView.NewPassword) ||
                    string.IsNullOrEmpty(accountAccessFromView.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Both new password and confirmation must be provided.");
                    return View("UpdateAccess", accountAccessFromView);
                }

                // Check if passwords match
                if (accountAccessFromView.NewPassword != accountAccessFromView.ConfirmPassword)
                {
                    ModelState.AddModelError("NewPassword", "New password does not match confirm password.");
                    ModelState.AddModelError("ConfirmPassword", "Confirm password does not match new password.");
                    return View("UpdateAccess", accountAccessFromView);
                }

                // Try to change password
                var changePassResult = await userManager.ChangePasswordAsync(
                    accountFromDB,
                    accountAccessFromView.OldPassword,
                    accountAccessFromView.NewPassword
                );

                if (!changePassResult.Succeeded)
                {
                    foreach (var error in changePassResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("UpdateAccess", accountAccessFromView);
                }
            }

            // Success - redirect to profile
            return RedirectToAction("Profile", "User", new { id = accountFromDB.Id });
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