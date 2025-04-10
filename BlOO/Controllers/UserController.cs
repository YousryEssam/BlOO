using BlOO.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Controllers
{
    public class UserController : Controller
    {
        private IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IPostLikeRepository postLikeRepository;
        private readonly IFollowRepository followRepository;
        private UserManager<ApplicationUser> _UserManager;
        private RoleManager<IdentityRole<int>> _RoleManager;
        private SignInManager<ApplicationUser> _SignInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager,
            IPostRepository postRepository, ICommentRepository commentRepository, IApplicationUserRepository applicationUserRepository, IPostLikeRepository postLikeRepository,IFollowRepository followRepository)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SignInManager = signInManager;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.postLikeRepository = postLikeRepository;
            this.followRepository = followRepository;
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
            ProfileVM.Posts = postRepository.GetAllPostsWithId(applicationUser.Id);
            foreach (var post in ProfileVM.Posts)
            {
                if (post.Comments == null)
                    post.Comments = new List<CommentWithUserDataViewModel>();
                List<PostLike> postLikes = postLikeRepository.GetAllPostLikesByPostId(post.Id);
                foreach (var postLike in postLikes)
                {
                    var user = postLike.User;
                    post.PostLikes.Add(new PostLikesViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfileImageUrl = user.ProfileImageUrl,
                        UserId = user.Id
                    });
                }
                var commentsFromDB = commentRepository.GetCommmentsByPostId(post.Id);

                foreach (var comment in commentsFromDB)
                {
                    var user = comment.User;

                    post.Comments.Add(new CommentWithUserDataViewModel
                    {
                        UserId = user.Id,
                        Content = comment.Content,
                        ProfileImageUrl = user.ProfileImageUrl,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CommentDate = comment.CommentDate,
                        LikeCount = comment.LikeCount
                    });
                }
            }
            ProfileVM.PostCount = ProfileVM.Posts.Count;

            return View(ProfileVM);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfileAsync()
        {
            var userFromDb = await _UserManager.GetUserAsync(User);
            if (userFromDb == null)
                return NotFound();

            var oldInfoUser = new EditProfileViewModel
            {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Bio = userFromDb.Bio,
                Email = userFromDb.Email
            };

            return View(oldInfoUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditProfileViewModel UserFromEdit)
        {
            ViewBag.Id = _UserManager.GetUserId(User);
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Followers(int id)
        {
            UserFollowersViewModel userFollowers = new UserFollowersViewModel();
            userFollowers.UserId = id;
            var followersIds = await followRepository.GetUserFollowersIds(id);
            List<ApplicationUser> followers = new List<ApplicationUser>();
            foreach (var f in followersIds) {
                followers.Add(applicationUserRepository.GetById(f));
            }
            userFollowers.Followers = followers;
            return View(userFollowers);
        }
            
        public async Task<IActionResult> Following(int id)
        {
            UserFollowingViewModel userFollowing = new UserFollowingViewModel();
            userFollowing.UserId = id;
            var followingIds = await followRepository.GetUserFollowingIds(id);
            List<ApplicationUser> followings = new List<ApplicationUser>();
            foreach (var f in followingIds) 
            {
                followings.Add(applicationUserRepository.GetById(f));
            }
            userFollowing.Following = followings;
            return View(userFollowing);
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
