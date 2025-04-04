using BlOO.Repositories;
using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Content("Hello, HomePage");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminPage()
        {
            return Content("Hello, AdminPage");
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

                postRepository.Save();

                return RedirectToAction("Post", "Post");
            }

            return View("EditPost", postVM);
        }


    }
}
