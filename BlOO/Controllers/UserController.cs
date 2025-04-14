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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userFromDb = await _UserManager.GetUserAsync(User);
            if (userFromDb == null)
            {
                return NotFound();
            }

            EditProfileViewModel oldInfoUser = new EditProfileViewModel
            {
                Bio = userFromDb.Bio,
                ProfileId = userFromDb.Id,
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName
            };

            return View(oldInfoUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditProfileViewModel userFromEdit)
        {
            if (!ModelState.IsValid)
            {
                return View("EditProfile", userFromEdit);
            }

            var userFromDb = await _UserManager.FindByIdAsync(userFromEdit.ProfileId.ToString());
            if (userFromDb == null)
            {
                return NotFound();
            }

            // Update basic info
            userFromDb.FirstName = userFromEdit.FirstName;
            userFromDb.LastName = userFromEdit.LastName;
            userFromDb.Bio = userFromEdit.Bio;

            // Handle profile image upload if provided
            if (userFromEdit.ProfileImage != null && userFromEdit.ProfileImage.Length > 0)
            {
                userFromDb.ProfileImageUrl = await UploadImageAsync(userFromEdit.ProfileImage, "/assets/profile-pictures/default-user.jpg");
            }

            // Handle cover image upload if provided
            if (userFromEdit.CoverImage != null && userFromEdit.CoverImage.Length > 0)
            {
                userFromDb.CoverImageUrl = await UploadImageAsync(userFromEdit.CoverImage, "/assets/profile-covers/default-cover.jpg");
            }

            var result = await _UserManager.UpdateAsync(userFromDb);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile", "User", new { id = userFromDb.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("EditProfile", userFromEdit);
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
            {
                return defaultImagePath;
            }

            if (imageFile.Length > 10 * 1024 * 1024) // 10MB limit
            {
                return defaultImagePath;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return defaultImagePath;
            }

            try
            {
                var fileName = $"{Guid.NewGuid()}{extension}";

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return "/uploads/" + fileName;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return defaultImagePath;
            }
        }
    }
}
