﻿using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName:"Admin")]
    public class TagController : Controller
    {
        private readonly IRepository<Tag> _repository;
        public TagController(IRepository<Tag> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _repository.GetAll().ToListAsync();
            return View(tags);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Tag tag)
        {
            if (ModelState.IsValid)
            {
                tag.CreatedAt = DateTime.Now;
                await _repository.AddAsync(tag);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            Tag tag = await _repository.GetAsync(id);
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Tag tag)
        {
            Tag updatedTag = await _repository.GetAsync(id);
            if (ModelState.IsValid)
            {
                updatedTag.Name = tag.Name;
                updatedTag.LastUpdatedAt = DateTime.Now;
                _repository.Update(tag);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            Tag tag = await _repository.GetAsync(id);
            _repository.Remove(tag);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}
