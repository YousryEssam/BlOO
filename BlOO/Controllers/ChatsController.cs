using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using BlOO.Repositories;
using Microsoft.AspNetCore.SignalR;
using BLOO.Hubs;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace BlOO.Controllers
{
    public class ChatsController : Controller
    {
        private UserManager<ApplicationUser> _UserManager;
        private SignInManager<ApplicationUser> _SignInManager;
        private readonly IFollowRepository followRepository;
        private readonly IApplicationUserRepository applicationUser;
        private readonly IMessageRepository messageRepository;
        private IHubContext<ChatMessageHub> ChatHubContext { get; }

        private int pageSize = 3;
        private int pageNumber = 1;

        public ChatsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IFollowRepository followRepository, IApplicationUserRepository applicationUser, IMessageRepository messageRepository, IHubContext<ChatMessageHub> chatHubContext)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            ChatHubContext = chatHubContext;
            this.followRepository = followRepository;
            this.applicationUser = applicationUser;
            this.messageRepository = messageRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            this.pageSize = pageSize;
            this.pageNumber = pageNumber;
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ChatsViewModel chatViewModel = await CreateChatsViewModel(user);
            return View(chatViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Chat(int id)
        {
            ApplicationUser? user = await _UserManager.GetUserAsync(User);
            ApplicationUser? target = await _UserManager.FindByIdAsync(id.ToString());
            ChatsViewModel chatViewModel = await CreateChatsViewModel(user, target);
            return View("Index", chatViewModel);
        }

        /**************************************Helpers******************************************/
        async Task<ChatsViewModel> CreateChatsViewModel(ApplicationUser user, ApplicationUser target = null)
        {
            ChatsViewModel chatViewModel;
            if (target != null)
            {
                chatViewModel = new ChatsViewModel(user, target);
                chatViewModel.OpenChat = messageRepository.GetChatMessages(user.Id, target.Id);
            }
            else
            {
                chatViewModel = new ChatsViewModel(user);
            }

            List<int> followsIDs = await followRepository.GetFollowedUsersAsync(user.Id, pageNumber, pageSize);
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
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    UserImgURL = applicationUser.ProfileImageUrl,
                    LastMessage = message?.Content ?? "No messages yet",
                    SendingDate = message?.SendingDate ?? DateTime.UtcNow
                });
            }
            chatViewModel.Conversations = chatViewModel.Conversations.OrderByDescending(c => c.SendingDate).ToList();
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.HasNextPage = followsIDs.Count == pageSize;
            ViewBag.HasPreviousPage = pageNumber > 1;
            return chatViewModel;
        }
    }
}