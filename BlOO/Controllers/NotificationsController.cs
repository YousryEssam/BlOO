using BlOO.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace BlOO.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private const int PAGE_SIZE = 10;
        private UserManager<ApplicationUser> _UserManager;
        private RoleManager<IdentityRole<int>> _RoleManager;
        private SignInManager<ApplicationUser> _SignInManager;
        private INotificationRepository notificationRepository;
        public NotificationsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager, INotificationRepository notificationRepository)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SignInManager = signInManager;
            this.notificationRepository = notificationRepository;
        }

        public async Task<IActionResult> Index(int page = 0)
        {
            List<NotificationsViewModel> notificationsViewModel = new List<NotificationsViewModel>();
            int userId = int.Parse(_UserManager.GetUserId(User));
            var UserNotifications = await notificationRepository.GetUserNotificationsById(userId, page, PAGE_SIZE);
            foreach (var notification in UserNotifications) {
                notificationsViewModel.Add(new NotificationsViewModel(notification));
            }

            int totalNotifications = notificationRepository.GetNumberOfUserNotificationsById(userId);
            int totalPages = totalNotifications / PAGE_SIZE;
            if (totalNotifications % PAGE_SIZE != 0)
            {
                totalPages++;
            }
            
            ViewBag.UserId = userId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            return View(notificationsViewModel);
        }
        public async Task<JsonResult> CheckUnreadNotifications()
        {
            int userId = int.Parse(_UserManager.GetUserId(User));
            bool unSeenNotification = await notificationRepository.HasUnseenNotificationByUserId(userId);
            return Json(unSeenNotification);
        }
    }
}
