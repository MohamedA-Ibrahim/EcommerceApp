using Domain.Entities;
using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryEndpoint _categoryEndpoint;
        public CategoryController(ICategoryEndpoint categoryEndpoint)
        {
            _categoryEndpoint = categoryEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryEndpoint.GetAll();
             return View(categories);  
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category cat)
        {
            await _categoryEndpoint.Create(cat);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = await _categoryEndpoint.GetById(id);
            if(categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            await _categoryEndpoint.UpdateAsync(category);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = await _categoryEndpoint.GetById(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _categoryEndpoint.DeleteAsync(id);

            return RedirectToAction("Index");

        }
    }
}
