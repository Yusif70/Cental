using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
