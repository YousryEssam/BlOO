using BlOO.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class MessageController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Message()
        {
            MessageViewModel messageViewModel = new MessageViewModel();
            return View(messageViewModel);
        }
    }
}
