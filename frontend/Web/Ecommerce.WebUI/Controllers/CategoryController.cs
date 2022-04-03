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
        public IActionResult Create(Category cat)
        {
            _categoryEndpoint.Create(cat);

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

        [HttpPut]
        public async Task<IActionResult> Edit(Category category)
        {
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
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _categoryEndpoint.DeleteAsync(id);

            return RedirectToAction("Index");

        }
    }
}
