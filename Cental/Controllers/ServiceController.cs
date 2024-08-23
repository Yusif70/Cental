using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
