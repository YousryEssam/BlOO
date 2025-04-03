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

        ///////////////////////////////// Helper Methods /////////////////////////////////////////
        
    }
}
