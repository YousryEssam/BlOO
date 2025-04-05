using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlOO.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> _UserManager;
        private RoleManager<IdentityRole<int>> _RoleManager;
        private SignInManager<ApplicationUser> _SignInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SignInManager = signInManager;
        }


        [Authorize]
        public async Task<IActionResult> Profile(int id)
        {
            ApplicationUser applicationUser = await _UserManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ProfileViewModel ProfileVM = new ProfileViewModel(applicationUser);
            return View(ProfileVM);
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditProfileViewModel UserFromEdit)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _UserManager.FindByEmailAsync(UserFromEdit.Email);
                if (userFromDb != null)
                {
                    if (UserFromEdit.ProfileImage != null)
                    {
                        var profileImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(UserFromEdit.ProfileImage.FileName);
                        var profileImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", profileImageFileName);

                        using (var stream = new FileStream(profileImagePath, FileMode.Create))
                        {
                            await UserFromEdit.ProfileImage.CopyToAsync(stream);
                        }

                        userFromDb.ProfileImageUrl = "/uploads/" + profileImageFileName;
                    }
                    else if(string.IsNullOrEmpty(UserFromEdit.ProfileImageUrl))
                    {
                         userFromDb.ProfileImageUrl = "/assets/profile-pictures/default-user.jpg";
                    }

                    
                    if (UserFromEdit.CoverImage != null)
                    {
                        var coverImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(UserFromEdit.CoverImage.FileName);
                        var coverImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", coverImageFileName);

                        using (var stream = new FileStream(coverImagePath, FileMode.Create))
                        {
                            await UserFromEdit.CoverImage.CopyToAsync(stream);
                        }

                        userFromDb.CoverImageUrl = "/uploads/" + coverImageFileName;
                    }
                    else if (string.IsNullOrEmpty(UserFromEdit.CoverImageUrl))
                    {
                        userFromDb.CoverImageUrl = "/assets/profile-covers/default-cover.jpg";
                    }


                    userFromDb.FirstName = UserFromEdit.FirstName;
                    userFromDb.LastName = UserFromEdit.LastName;
                    userFromDb.Bio = UserFromEdit.Bio;

                    
                    var changePass = await _UserManager.ChangePasswordAsync(userFromDb, UserFromEdit.OldPassword, UserFromEdit.NewPassword);
                    if (!changePass.Succeeded)
                    {
                        foreach (var error in changePass.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                    var updateUser = await _UserManager.UpdateAsync(userFromDb);
                    if (updateUser.Succeeded)
                    {
                        return RedirectToAction("Profile",new {id = UserFromEdit.Id});
                    }

                    foreach (var error in updateUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("EditProfile", UserFromEdit);
        }


        ///////////////////////////////// Helper Methods /////////////////////////////////////////

    }
}
