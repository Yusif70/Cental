using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Controllers
{
    public class AboutController : Controller
    {
        private readonly IRepository<Employee> _repository;
        public AboutController(IRepository<Employee> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _repository.GetAll().ToListAsync();
            return View(employees);
        }
    }
}
