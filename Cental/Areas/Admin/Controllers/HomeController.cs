using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
