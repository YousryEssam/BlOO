using BlOO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class PostController : Controller
    {
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


    }
}
