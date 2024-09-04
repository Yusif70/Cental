using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IRepository<Employee> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _repository.GetAll().ToListAsync();
            return View(employees);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            string fileName = Guid.NewGuid().ToString() + employee.File?.FileName;
            string path = _webHostEnvironment.WebRootPath + "/admin/images/employees/" + fileName;
            if (ModelState.IsValid)
            {
                employee.CreatedAt = DateTime.Now;
                employee.Image = "defaultPfp.png";
                if (employee.File != null)
                {
                    employee.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await employee.File.CopyToAsync(fileStream);
                }
                await _repository.AddAsync(employee);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            Employee employee = await _repository.GetAsync(id);
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            Employee updatedEmployee = await _repository.GetAsync(id);
            string basePath = _webHostEnvironment.WebRootPath + "/admin/images/employees/" + updatedEmployee.Image;
            if (ModelState.IsValid)
            {
                if (updatedEmployee.Image != "defaultPfp.png")
                {
                    System.IO.File.Delete(basePath);
                }
                if (employee.File != null)
                {
                    string fileName = Guid.NewGuid().ToString() + employee.File?.FileName;
                    string path = _webHostEnvironment.WebRootPath + "/admin/images/employees/" + fileName;
                    updatedEmployee.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await employee.File!.CopyToAsync(fileStream);
                }
                updatedEmployee.Name = employee.Name;
                updatedEmployee.Surname = employee.Surname;
                updatedEmployee.Profession = employee.Profession;
                updatedEmployee.LastUpdatedAt = DateTime.Now;
                _repository.Update(updatedEmployee);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View(employee);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Employee employee = await _repository.GetAsync(id);
            if (employee.Image != "defaultPfp.png")
            {
                string path = _webHostEnvironment.WebRootPath + "/admin/images/employees/" + employee.Image;
                System.IO.File.Delete(path);
            }
            _repository.Remove(employee);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}
