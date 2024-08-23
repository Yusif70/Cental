using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class FeatureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
