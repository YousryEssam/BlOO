using Microsoft.AspNetCore.Mvc;

namespace BlOO.ViewComponents
{
    public class PostInputViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public PostInputViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            string profileImageUrl = user?.ProfileImageUrl ?? "/assets/profile-pictures/default-user.jpg";
            return View("Default", profileImageUrl);
        }
    }
}
