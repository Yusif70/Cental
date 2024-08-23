using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
