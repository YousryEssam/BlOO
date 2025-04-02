using BlOO.Models;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string Email)
        {
            ApplicationUser applicationUser = await _UserManager.FindByEmailAsync(Email);
            if(applicationUser == null)
            {
                return NotFound();
            }
            ProfileViewModel ProfileVM = GetProfileViewModel(applicationUser);
            return View(ProfileVM);
        }

        ///////////////////////////////// Helper Methods /////////////////////////////////////////
        
        private ProfileViewModel GetProfileViewModel(ApplicationUser user)
        {
            ProfileViewModel ProfileVM = new ProfileViewModel();
            ProfileVM.Bio = user.Email;
            ProfileVM.LastName = user.LastName;
            ProfileVM.UserName = user.UserName;
            ProfileVM.FirstName = user.FirstName;
            ProfileVM.FollowersCount = user.FollowersCount;
            ProfileVM.FollowingCount = user.FollowingCount;
            ProfileVM.PostCount = user.PostCount;
            ProfileVM.CoverImageUrl = user.CoverImageUrl;
            ProfileVM.ProfileImageUrl = user.ProfileImageUrl;



            return ProfileVM;
        }
    }
}
