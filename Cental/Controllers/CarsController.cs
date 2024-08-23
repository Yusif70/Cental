using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class CarsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
