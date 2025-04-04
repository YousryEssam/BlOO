using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using BlOO.Repositories;

namespace BlOO.Controllers
{
    public class ChatsController : Controller
    {
        private UserManager<ApplicationUser> _UserManager;
        private SignInManager<ApplicationUser> _SignInManager;
        private readonly IFollowRepository followRepository;
        private readonly IApplicationUserRepository applicationUser;
        private readonly IMessageRepository messageRepository;

        public ChatsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IFollowRepository followRepository, IApplicationUserRepository applicationUser, IMessageRepository messageRepository)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            this.followRepository = followRepository;
            this.applicationUser = applicationUser;
            this.messageRepository = messageRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=3)
        {
            if (pageNumber <= 1) {
                pageNumber = 1;
            }
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ChatsViewModel chatViewModel = new ChatsViewModel(user);

            List<int> followsIDs = await followRepository.GetFollowedUsersAsync(user.Id,pageNumber,pageSize);
            List<ApplicationUser> users = new List<ApplicationUser>();

            for (int i = 0; i < followsIDs.Count; i++)
            {
                ApplicationUser user1 = applicationUser.GetById(followsIDs[i]);
                users.Add(user1);

            }

            foreach (ApplicationUser applicationUser in users)
            {
                Message? message = messageRepository.getLastMessage(user.Id, applicationUser.Id);

                chatViewModel.Conversations.Add(new ConversationViewModel
                {
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    UserImgURL = applicationUser.ProfileImageUrl,
                    LastMessage = message?.Content ?? "No messages yet",
                    SendingDate = message?.SendingDate ?? DateTime.UtcNow
                });
            }

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.HasNextPage = followsIDs.Count == pageSize;
            ViewBag.HasPreviousPage = pageNumber > 1;
            return View(chatViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Chat(int id)
        {
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ApplicationUser? target = await _UserManager.FindByIdAsync(id.ToString());
            ChatsViewModel chatViewModel = new ChatsViewModel(user, target);

            return View("Index", chatViewModel);
        }
    }
}