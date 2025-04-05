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
            if (!ModelState.IsValid)
                return View("EditProfile", UserFromEdit);

            var userFromDb = await _UserManager.FindByEmailAsync(UserFromEdit.Email);
            if (userFromDb == null)
                return NotFound();

            userFromDb.ProfileImageUrl = await UploadImageAsync(UserFromEdit.ProfileImage, "/assets/profile-pictures/default-user.jpg");
            userFromDb.CoverImageUrl = await UploadImageAsync(UserFromEdit.CoverImage, "/assets/profile-covers/default-cover.jpg");
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
                return View("EditProfile", UserFromEdit);
            }

            var result = await _UserManager.UpdateAsync(userFromDb);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile", new { id = userFromDb.Id });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("EditProfile", UserFromEdit);
        }



        ///////////////////////////////// Helper Methods /////////////////////////////////////////
        private async Task<string> UploadImageAsync(IFormFile imageFile, string defaultImagePath)
        {
            if (imageFile == null || imageFile.Length == 0)
                return defaultImagePath;

            if (imageFile.Length > 2 * 1024 * 1024)
                return defaultImagePath;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return defaultImagePath;

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/uploads/" + fileName;
        }

    }
}
