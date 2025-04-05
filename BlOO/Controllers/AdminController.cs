using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class AdminController : Controller
    {

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return Content("Hello, AdminPage");
        }

      
    }
}
