using BlOO.Repositories;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlOO.Controllers
{
    public class PostController : Controller
    {
        IPostRepository postRepository;
        public PostController(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        [Authorize]
        public IActionResult HomePage()
        {
            List<PostViewModel> posts = postRepository.GetAllPostsWithUsers();
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
                postViewModel.ImgUrl = "/assets/post-pictures/" + uniqueFileName;

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

            return PartialView("_PostComponent", postViewModel);
        }



        public IActionResult Post()
        {
            PostViewModel postViewModel = new PostViewModel();
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
        public IActionResult SaveEditedPost(PostViewModel postVM)
        {
            var postFromDB = postRepository.GetById(postVM.Id);
            if (postFromDB == null)
            {
                return NotFound(); 
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
    }
}
