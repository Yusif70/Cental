using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class _404Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
