using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class ChatsController : Controller
    {
        public IActionResult Index()
        {
            ChatsViewModel chatViewModel = new ChatsViewModel();
            return View(chatViewModel);
        }
    }
}
