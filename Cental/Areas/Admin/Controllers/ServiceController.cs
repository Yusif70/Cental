using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
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
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Service service)
        {
            if (ModelState.IsValid)
            {
                service.CreatedAt = DateTime.Now;
                await _repository.AddAsync(service);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View(service);
        }
        public async Task<IActionResult> Update(int id)
        {
            Service service = await _repository.GetAsync(id);
            return View(service);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Service service)
        {
            Service updatedService = await _repository.GetAsync(id);
            if (ModelState.IsValid)
            {
                updatedService.Title = service.Title;
                updatedService.Description = service.Description;
                updatedService.Icon = service.Icon;
                updatedService.LastUpdatedAt = DateTime.Now;
                _repository.Update(updatedService);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View(service);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoveAsync(id);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}
