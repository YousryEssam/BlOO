using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlOO.Controllers
{
    public class ChatsController : Controller
    {
        private UserManager<ApplicationUser> _UserManager;
        private SignInManager<ApplicationUser> _SignInManager;

        public ChatsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ChatsViewModel chatViewModel = new ChatsViewModel(user);
            return View(chatViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Chat(int id)
        {
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ApplicationUser? target = await _UserManager.FindByIdAsync(id.ToString());
            ChatsViewModel chatViewModel = new ChatsViewModel(user, target);
            return View("Index",chatViewModel);
        }
    }
}
