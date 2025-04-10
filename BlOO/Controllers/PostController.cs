using BlOO.Repositories;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;
using BlOO.Models;
using Microsoft.AspNetCore.Identity;

namespace BlOO.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private SignInManager<ApplicationUser> _SignInManager;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IPostLikeRepository postLikeRepository;
        private readonly IPostReportRepository postReportRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository,
            SignInManager<ApplicationUser> signInManager, IApplicationUserRepository applicationUserRepository,
            IPostLikeRepository postLikeRepository, IPostReportRepository postReportRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            _SignInManager = signInManager;
            this.applicationUserRepository = applicationUserRepository;
            this.postLikeRepository = postLikeRepository;
            this.postReportRepository = postReportRepository;
        }
        [Authorize]
        public IActionResult HomePage()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<PostViewModel> posts = postRepository.GetPostsUsersFollow(userId);

            foreach (var post in posts)
            {
                if (post.Comments == null)
                    post.Comments = new List<CommentWithUserDataViewModel>();

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

            return View("Post", posts);
        }

        public IActionResult Explore()
        {
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            List<PostViewModel> posts = postRepository.GetRandomPosts(UserId);
            foreach (var post in posts)
            {
                if (post.Comments == null)
                    post.Comments = new List<CommentWithUserDataViewModel>();

                var commentsFromDB = commentRepository.GetCommmentsByPostId(post.Id);

                List<PostLike> postLikes = postLikeRepository.GetAllPostLikesByPostId(post.Id);
                foreach (var postLike in postLikes)
                {
                    var user = postLike.User;
                    post.PostLikes.Add(new PostLikesViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfileImageUrl = user.ProfileImageUrl,
                        UserId=user.Id
                    });
                }
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



            return View("Post", posts);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DataFromAddPost(PostViewModel postViewModel, IFormFile Image)
        {
            if (Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/post-pictures");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
                postViewModel.ImgUrl = uniqueFileName;

            }
            string? UserName = $"{User.FindFirst("FirstName")?.Value} {User.FindFirst("LastName")?.Value}";

            postViewModel.UserName = UserName;
            postViewModel.UserImgUrl = User.FindFirst("imgUrl")?.Value;

            Post post = new Post()
            {

                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Content = postViewModel.Content,
                ImgUrl = postViewModel.ImgUrl,
                LikeCount = postViewModel.LikeCount,
                CommentCount = postViewModel.CommentCount,
                RepostCount = postViewModel.RepostCount,

            };

            postRepository.Insert(post);
            postRepository.Save();

            postViewModel.Id = post.Id;

            return PartialView("_PostComponent", postViewModel);
        }


        public IActionResult Post()
        {
            PostViewModel postViewModel = new PostViewModel();
            List<Comment> CommentsfromDB = commentRepository.GetCommmentsByPostId(postViewModel.Id);

            List<PostLike> postLikes = postLikeRepository.GetAllPostLikesByPostId(postViewModel.Id);
            foreach (var postLike in postLikes)
            {
                var user = postLike.User;
                postViewModel.PostLikes.Add(new PostLikesViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileImageUrl = user.ProfileImageUrl,
                    UserId = user.Id
                });
            }
            foreach (Comment comment in CommentsfromDB)
            {
                ApplicationUser user = applicationUserRepository.GetById(comment.UserId);
                postViewModel.Comments.Add(new CommentWithUserDataViewModel
                {
                    UserId = user.Id,
                    Content = comment.Content,
                    ProfileImageUrl = user.ProfileImageUrl,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CommentDate = comment.CommentDate,
                }
                    );
            }


            return View(postViewModel);
        }

        public IActionResult EditPost(int id)
        {
            var postRequest = postRepository.GetById(id);
            if (postRequest == null)
            {
                return NotFound(); 
            }
            var postVM = new PostViewModel
            {
                Id = postRequest.Id,
                OwnerId = postRequest.UserId,
                Content = postRequest.Content,
                ImgUrl = postRequest.ImgUrl,
                LikeCount = postRequest.LikeCount,
                CommentCount = postRequest.CommentCount,
                RepostCount = postRequest.RepostCount,
                UserName = postRequest.User?.UserName ,
                UserImgUrl = postRequest.User?.ProfileImageUrl 
            };

            return View("EditPost", postVM);
        }


        [HttpPost]
        public IActionResult SaveEditedPost(PostViewModel postVM, IFormFile Image)
        {
            var postFromDB = postRepository.GetById(postVM.Id);
            if (postFromDB == null)
            {
                return NotFound(); 
            }

            if (Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/post-pictures");

                if (!string.IsNullOrEmpty(postFromDB.ImgUrl))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, postFromDB.ImgUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath); // مسح الصورة القديمة
                    }
                }
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
                postVM.ImgUrl = uniqueFileName;

            }

            if (ModelState.IsValid)
            {
                postFromDB.Content = postVM.Content;
                postFromDB.ImgUrl = postVM.ImgUrl;
                postRepository.Update(postFromDB);
                postRepository.Save();
                return RedirectToAction("Profile", "User", new { id = postVM.OwnerId });
            }
            return View("EditPost", postVM);
        }


        public IActionResult DeletePost(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var Image = postRepository.GetById(id).ImgUrl;
         
            if (Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/post-pictures");

                if (!string.IsNullOrEmpty(Image))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath); // مسح الصورة القديمة
                    }
                }
            }
            postRepository.DeleteById(id);
            postRepository.Save();

            return RedirectToAction("Profile", "User", new { id = userId });

        }

        public IActionResult ReportPost(int id , ReportReason reason)
        {
            int userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            PostReport postReport = new PostReport()
            {
                   ReporterId = userid,
                   PostId = id,
                   Reason = reason,
     
                
            };
            postReportRepository.Insert(postReport);
            postReportRepository.Save();
            return RedirectToAction("HomePage", "Post");
        }

    }
}
