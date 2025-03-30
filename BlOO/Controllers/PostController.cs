using BlOO.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class PostController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}



        public IActionResult Post()
        {

            PostViewModel postViewModel = new PostViewModel();
            return View(postViewModel);
        }


    }
}
