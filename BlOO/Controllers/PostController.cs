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
        [Authorize]     
        public IActionResult HomePage()
        {
            List<PostViewModel> posts = postRepository.GetAllPostsWithUsers();
            return View("Post", posts);
        }
        public IActionResult DataFromAddPost(PostViewModel postViewModel, IFormFile Image)
        {
            if (Image != null )
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
                Content= postViewModel.Content,
                ImgUrl = postViewModel.ImgUrl,
                LikeCount = postViewModel.LikeCount,
                CommentCount = postViewModel.CommentCount,
                RepostCount=postViewModel.RepostCount,

            };

            postRepository.Insert(post);
            postRepository.Save();

            return PartialView("_ShowPosts", postViewModel);
        }


    }
}
