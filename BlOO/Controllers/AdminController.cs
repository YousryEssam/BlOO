using BlOO.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlOO.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPostReportRepository postReportRepository;

        public AdminController(IPostReportRepository postReportRepository)
        {
            this.postReportRepository = postReportRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {

            ViewData["HideNavbar"] = "true";
            return View("Admin");
        }


      
    }
}
