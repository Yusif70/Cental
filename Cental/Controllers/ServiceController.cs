using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IRepository<Service> _repository;
        public ServiceController(IRepository<Service> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _repository.GetAll().ToListAsync();
            return View(services);
        }
    }
}
